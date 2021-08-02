using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public Task CreateMessageAsync(CreateMessage createMessage);
        public Task<Conversation> CreateConversationAsync(CreateConversation createConversation);
        public Task<Conversation> GetConversationByConnectionIdAsync(string connectionId);
        public Conversation GetConversationById(Guid id);
        public Task UpdateConversationAsync(Conversation conversation, ConversationStatus conversationStatus);
        public IQueryable<Conversation> GetNewConversationsQuery();
        public Task<HashEntry[]> GetAllConnectionsAsync();
        public Task AddConnectionAsync(string connectionId, string conversationId);
        public Task RemoveConnectionAsync(string connectionId);

    }
}
