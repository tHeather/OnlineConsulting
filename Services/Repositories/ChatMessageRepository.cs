using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatMessageRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<ChatMessage> CreateMessageAsync(CreateMessage createMessage)
        {
            var message = new ChatMessage
            {
                ConversationId = createMessage.ConversationId,
                Content = createMessage.Content,
                CreateDate = DateTime.UtcNow,
                IsFromClient = createMessage.IsFromClient
            };

            _dbContext.ChatMessages.Add(message);

            var conversation = await _dbContext
                                       .Conversations
                                       .FirstOrDefaultAsync(c => c.Id == createMessage.ConversationId);

            conversation.LastMessageId = message.Id;

            await _dbContext.SaveChangesAsync();

            return message;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesForConversationById(Guid conversationId)
        {
            var conversation = await _dbContext.Conversations
                                        .Include(c => c.ChatMessages)
                                        .SingleOrDefaultAsync(c => c.Id == conversationId);

            return conversation.ChatMessages;
        }

    }
}
