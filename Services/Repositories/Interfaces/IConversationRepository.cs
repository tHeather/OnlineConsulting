using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Models.ValueObjects.Statistic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        public Task<Conversation> CreateConversationAsync(CreateConversation createConversation);
        public Task<Conversation> GetConversationByIdAsync(Guid id);
        public Task CloseConversationAsync(Conversation conversation);
        public Task<bool> AssignConsultantToConversation(
                                        Conversation conversation, string consultantId, byte[] rowVersion);
        public IQueryable<Conversation> GetNewConversationsQuery();
        public IQueryable<Conversation> GetInProgressConversationsForConsultantQuery(string consultantId);
        public Task<ConversationStatistics> GetConversationsStatistics(
                                                ConversationStatisticsParams conversationStatisticsParams);
        public Task CloseUnusedConversationsAsync();
        public IQueryable<Conversation> GetFilteredAndSortedConversationsQuery(
                                                    ConversationFilters filters, bool isAscending = false);
    }
}
