using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Models.ValueObjects.Statistic;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
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
            var filterQuery = _dbContext.Conversations
                                            .Where(
                                                c => c.CreateDate >= conversationStatisticsParams.StartDate &&
                                                c.CreateDate < conversationStatisticsParams.EndDate
                                            );

            if (conversationStatisticsParams.Domain != null) filterQuery.Where(c => c.Host == conversationStatisticsParams.Domain);

            var allConversationsQuery = filterQuery.GroupBy(
                            c => c.CreateDate.Hour < hoursOffset ? c.CreateDate.Date : c.CreateDate.Date.AddDays(1),
                            (date, conversationsDates) => new DailyConversationsNumber
                            {
                                Date = date,
                                Count = conversationsDates.Count()
                            }); ;

            var servedConversationsQuery = filterQuery.Where(
                                                    s => s.Status == ConversationStatus.DONE &&
                                                    s.ConsultantId != null
                                                    ).GroupBy(
                                     c => c.CreateDate.Hour < hoursOffset ? c.CreateDate.Date : c.CreateDate.Date.AddDays(1),
                                    (date, conversationsDates) => new DailyConversationsNumber
                                    {
                                        Date = new DateTime(date.Year, date.Month, date.Day, hoursOffset, 0, 0),
                                        Count = conversationsDates.Count()
                                    }); ;

            var notServedConversationsQuery = filterQuery.Where(
                                        s => s.Status == ConversationStatus.DONE &&
                                        s.ConsultantId == null
                                        ).GroupBy(
                         c => c.CreateDate.Hour < hoursOffset ? c.CreateDate.Date : c.CreateDate.Date.AddDays(1),
                        (date, conversationsDates) => new DailyConversationsNumber
                        {
                            Date = new DateTime(date.Year, date.Month, date.Day, hoursOffset, 0, 0),
                            Count = conversationsDates.Count()
                        }); ;

            var consultantsJoiningTimes = await filterQuery.Select(c => c.StartDate - c.CreateDate).ToListAsync();


            var averageTimeConsultantJoining = consultantsJoiningTimes.Count > 0 ?
                                                    consultantsJoiningTimes.Average(t => t.Value.Ticks) :
                                                    0;

            var averageTimeConsultantJoiningTimespan = new TimeSpan((long)averageTimeConsultantJoining);



            var conversationsDurations = await filterQuery.Select(c => c.EndDate - c.StartDate).ToListAsync();


            var averageConversationDuration = conversationsDurations.Count > 0 ?
                                                    conversationsDurations.Average(t => t.Value.Ticks) :
                                                    0;

            var averageConversationDurationTimespan = new TimeSpan((long)averageConversationDuration);


            return new ConversationStatistics
            {
                AllConversations = await allConversationsQuery.ToListAsync(),
                ServedConversations = await servedConversationsQuery.ToListAsync(),
                NotServedConversations = await notServedConversationsQuery.ToListAsync(),
                AverageTimeConsultantJoining = averageTimeConsultantJoiningTimespan,
                AverageConversationDuration = averageConversationDurationTimespan
            };
        }

    }
}
