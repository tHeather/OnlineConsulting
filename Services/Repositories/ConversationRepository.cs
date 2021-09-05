using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Models.ValueObjects.Statistic;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Services.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ConversationRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
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

        public async Task CloseConversationAsync(Conversation conversation)
        {
            conversation.Status = ConversationStatus.DONE;
            conversation.EndDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AssignConsultantToConversation(
            Conversation conversation, string consultantId, byte[] rowVersion)
        {
            try
            {
                conversation.Status = ConversationStatus.IN_PROGRESS;
                conversation.ConsultantId = consultantId;
                conversation.StartDate = DateTime.UtcNow;
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

        public IQueryable<Conversation> GetInProgressConversationsForConsultantQuery(string consultantId)
        {
            return _dbContext.Conversations
                        .Where(c => c.Status == ConversationStatus.IN_PROGRESS && c.ConsultantId == consultantId)
                        .Include(c => c.LastMessage);
        }

        public async Task<ConversationStatistics> GetConversationsStatistics(ConversationStatisticsParams conversationStatisticsParams)
        {
            var hoursOffset = conversationStatisticsParams.StartDate.Hour;
            var filterQuery = FilterStatisticsQuery(conversationStatisticsParams);

            var allConversationsByDayQuery = CountConversationsByDayQuery(filterQuery, hoursOffset);


            var servedConversationsQuery = filterQuery.Where(
                                                    s => s.Status == ConversationStatus.DONE &&
                                                    s.ConsultantId != null
                                                    );
            var servedConversationsByDayQuery = CountConversationsByDayQuery(servedConversationsQuery, hoursOffset);


            var notServedConversationsQuery = filterQuery.Where(
                                        s => s.Status == ConversationStatus.DONE &&
                                        s.ConsultantId == null
                                        );
            var notServedConversationsByDayQuery = CountConversationsByDayQuery(notServedConversationsQuery, hoursOffset);


            var inProgressConversationsQuery = filterQuery.Where(
                                        s => s.Status == ConversationStatus.IN_PROGRESS
                                        );

            var newConversationsQuery = filterQuery.Where(
                            s => s.Status == ConversationStatus.NEW
                            );

            var consultantsJoiningTimes = await filterQuery
                                                    .Where(c => c.StartDate != null)
                                                    .Select(c => c.StartDate - c.CreateDate).ToListAsync();
            var averageTimeConsultantJoiningTimespan = CalcAverageTime(consultantsJoiningTimes);

            var conversationsDurations = await filterQuery
                                                .Where(c => c.StartDate != null && c.EndDate != null)
                                                .Select(c => c.EndDate - c.StartDate).ToListAsync();
            var averageConversationDurationTimespan = CalcAverageTime(conversationsDurations);

            return new ConversationStatistics
            {
                AllConversations = await allConversationsByDayQuery.ToListAsync(),
                ServedConversations = await servedConversationsByDayQuery.ToListAsync(),
                NotServedConversations = await notServedConversationsByDayQuery.ToListAsync(),
                InProgressConversationsNumber = await inProgressConversationsQuery.CountAsync(),
                NewConversationsNumber = await newConversationsQuery.CountAsync(),
                AverageTimeConsultantJoining = averageTimeConsultantJoiningTimespan,
                AverageConversationDuration = averageConversationDurationTimespan
            };
        }

        private IQueryable<Conversation> FilterStatisticsQuery(ConversationStatisticsParams conversationStatisticsParams)
        {
            var filterQuery = _dbContext.Conversations.Where(
                                                c => c.CreateDate >= conversationStatisticsParams.StartDate &&
                                                c.CreateDate < conversationStatisticsParams.EndDate
                                            );

            if (conversationStatisticsParams.Domain != null) filterQuery.Where(
                                                                c => c.Host == conversationStatisticsParams.Domain
                                                                );
            return filterQuery;
        }

        private IQueryable<DailyConversationsNumber> CountConversationsByDayQuery(
            IQueryable<Conversation> sourceQuery, int hoursOffset
            )
        {
            return sourceQuery.GroupBy(
                c => c.CreateDate.Hour < hoursOffset ? c.CreateDate.Date : c.CreateDate.Date.AddDays(1),
                (date, conversationsDates) => new DailyConversationsNumber
                {
                    Date = date,
                    Count = conversationsDates.Count()
                }); ;
        }

        private TimeSpan CalcAverageTime(List<TimeSpan?> source)
        {
            var averageTimeConsultantJoining = source.Count > 0 ?
                                                    source.Average(t => t.Value.Ticks) :
                                                    0;
            return new TimeSpan((long)averageTimeConsultantJoining);
        }

    }
}
