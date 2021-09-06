using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IChatMessageRepository
    {
        public Task<ChatMessage> CreateMessageAsync(CreateMessage createMessage);
        public Task<IEnumerable<ChatMessage>> GetAllMessagesForConversationById(Guid conversationId);
    }
}
