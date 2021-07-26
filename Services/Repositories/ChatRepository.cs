using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Services.Repositories.Interfaces;
using StackExchange.Redis;
using System;
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
                Origin = createMessage.Origin,
                Content = createMessage.Content,
                CreateDate = DateTime.UtcNow
            };

            _dbContext.ChatMessages.Add(message);

            createMessage.Conversation.LastMessageId = message.Id;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Conversation> CreateConversationAsync()
        {
            var conversation = new Conversation
            {
                CreateDate = DateTime.UtcNow,
                Status = ConversationStatus.NEW
            };

            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            return conversation;
        }

        public Conversation GetConversationById(Guid id)
        {
            return _dbContext.Conversations.SingleOrDefault(c => c.Id == id);
        }

        public async Task<Conversation> GetConversationByConnectionIdAsync(string connectionId)
        {
            var database = _multiplexer.GetDatabase();
            var conversationId = await database.HashGetAsync(SIGNALR_CONNECTIONS, connectionId);
            return GetConversationById(Guid.Parse(conversationId.ToString()));
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

        public async Task<HashEntry[]> GetAllConnectionsAsync()
        {
            var database = _multiplexer.GetDatabase();
            return await database.HashGetAllAsync(SIGNALR_CONNECTIONS);
        }

    }
}
