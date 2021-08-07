using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Services.Repositories.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConnectionMultiplexer _multiplexer;
        const string SIGNALR_CONNECTIONS = "signalr-connections";

        public ChatRepository(ApplicationDbContext applicationDbContext, IConnectionMultiplexer multiplexer)
        {
            _dbContext = applicationDbContext;
            _multiplexer = multiplexer;
        }

        public async Task CreateMessageAsync(CreateMessage createMessage)
        {
            var message = new ChatMessage
            {
                ConversationId = createMessage.Conversation.Id,
                Content = createMessage.Content,
                CreateDate = DateTime.UtcNow
            };

            _dbContext.ChatMessages.Add(message);

            createMessage.Conversation.LastMessageId = message.Id;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Conversation> CreateConversationAsync(CreateConversation createConversation)
        {
            var conversation = new Conversation
            {
                CreateDate = DateTime.UtcNow,
                Status = ConversationStatus.NEW,
                Host = createConversation.Host,
                Path = createConversation.Path
            };

            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            return conversation;
        }

        public Conversation GetConversationById(Guid id)
        {
            return _dbContext.Conversations.SingleOrDefault(c => c.Id == id);
        }

        public async Task ChangeConversationStatusAsync(Conversation conversation, ConversationStatus conversationStatus)
        {
            conversation.Status = conversationStatus;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ChangeConversationStatusConcurrencySafeAsync(
            Conversation conversation, ConversationStatus conversationStatus, byte[] rowVersion)
        {
            try
            {
                conversation.Status = conversationStatus;
                _dbContext.Entry(conversation).OriginalValues["RowVersion"] = rowVersion;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<Conversation> GetConversationByClientConnectionIdAsync(string ClientConnectionId)
        {
            var conversationId = await GetConversationIdByClientConnectionIdAsync(ClientConnectionId);
            if (conversationId == null) return null;
            return GetConversationById((Guid)conversationId);
        }

        public async Task<Guid?> GetConversationIdByClientConnectionIdAsync(string ClientConnectionId)
        {
            var database = _multiplexer.GetDatabase();
            var conversationId = await database.HashGetAsync(SIGNALR_CONNECTIONS, ClientConnectionId);
            if (conversationId.IsNullOrEmpty) return null;
            return Guid.Parse(conversationId.ToString());
        }

        public IQueryable<Conversation> GetNewConversationsQuery()
        {
            return _dbContext.Conversations.Where(c => c.Status == ConversationStatus.NEW).Include(c => c.LastMessage);
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesForConversationByClientConnectionId(string connectionId)
        {
            var conversationId = await GetConversationIdByClientConnectionIdAsync(connectionId);
            var conversation = _dbContext.Conversations.Where(c => c.Id == conversationId).Include(c => c.ChatMessages).SingleOrDefault();
            return conversation.ChatMessages;
        }

        public async Task<HashEntry[]> GetAllConnectionsAsync()
        {
            var database = _multiplexer.GetDatabase();
            return await database.HashGetAllAsync(SIGNALR_CONNECTIONS);
        }

        public async Task AddConnectionAsync(string connectionId, string conversationId)
        {
            var database = _multiplexer.GetDatabase();
            await database.HashSetAsync(SIGNALR_CONNECTIONS, new HashEntry[] { new HashEntry(connectionId, conversationId) });
        }

        public async Task RemoveConnectionAsync(string connectionId)
        {
            var database = _multiplexer.GetDatabase();
            await database.HashDeleteAsync(SIGNALR_CONNECTIONS, connectionId);
        }

    }
}
