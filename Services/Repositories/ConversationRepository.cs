using Microsoft.EntityFrameworkCore;
using OnlineConsulting.Data;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
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

        public async Task ChangeConversationStatusAsync(Conversation conversation, ConversationStatus conversationStatus)
        {
            conversation.Status = conversationStatus;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AssignConsultantToConversation(
            Conversation conversation, string consultantId, byte[] rowVersion)
        {
            try
            {
                conversation.Status = ConversationStatus.IN_PROGRESS;
                conversation.ConsultantId = consultantId;
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

    }
}
