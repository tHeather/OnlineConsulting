using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public Task CreateMessageAsync(CreateMessage createMessage);
        public Task<Conversation> CreateConversationAsync();
        public Task AddConnectionAsync(string connectionId, string conversationId);
        public Task RemoveConnectionAsync(string connectionId);
        public Task<HashEntry[]> GetAllConnectionsAsync();
        public Task<Conversation> GetConversationByConnectionIdAsync(string connectionId);
        public Conversation GetConversationById(Guid id);
    }
}
