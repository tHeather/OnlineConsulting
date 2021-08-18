using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<ChatMessage> CreateMessageAsync(CreateMessage createMessage)
        {
            var message = new ChatMessage
            {
                ConversationId = createMessage.Conversation.Id,
                Content = createMessage.Content,
                CreateDate = DateTime.UtcNow,
                IsFromClient = createMessage.IsFromClient
            };

            _dbContext.ChatMessages.Add(message);

            createMessage.Conversation.LastMessageId = message.Id;

            await _dbContext.SaveChangesAsync();

            return message;
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

        public Task<Conversation> GetConversationByIdAsync(Guid id)
        {
            return _dbContext.Conversations.SingleOrDefaultAsync(c => c.Id == id);
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

        public IQueryable<Conversation> GetNewConversationsQuery()
        {
            return _dbContext.Conversations
                        .Where(c => c.Status == ConversationStatus.NEW)
                        .Include(c => c.LastMessage);
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesForConversationById(Guid conversationId)
        {
            var conversation = await _dbContext.Conversations
                                        .Where(c => c.Id == conversationId)
                                        .Include(c => c.ChatMessages)
                                        .SingleOrDefaultAsync();

            return conversation.ChatMessages;
        }

    }
}
