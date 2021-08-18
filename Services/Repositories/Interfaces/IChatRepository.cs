using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public Task<ChatMessage> CreateMessageAsync(CreateMessage createMessage);
        public Task<Conversation> CreateConversationAsync(CreateConversation createConversation);
        public Task<Conversation> GetConversationByIdAsync(Guid id);
        public Task ChangeConversationStatusAsync(Conversation conversation, ConversationStatus conversationStatus);
        public Task<bool> ChangeConversationStatusConcurrencySafeAsync(
            Conversation conversation, ConversationStatus conversationStatus, byte[] rowVersion);
        public IQueryable<Conversation> GetNewConversationsQuery();
        public Task<IEnumerable<ChatMessage>> GetAllMessagesForConversationById(Guid conversationId);
    }
}
