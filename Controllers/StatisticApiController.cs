using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Attributes;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.ValueObjects.Statistic;
using OnlineConsulting.Services.Repositories.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [TypeFilter(typeof(ValidateSubscriptionAttribute))]
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
    [Route("api/statistics")]
    [ApiController]
    public class StatisticApiController : ControllerBase
    {

        private readonly IConversationRepository _conversationRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public StatisticApiController(
            IConversationRepository conversationRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _conversationRepository = conversationRepository;
            _subscriptionRepository = subscriptionRepository;
        }


        [HttpGet("get-statistics")]
        public async Task<ConversationStatistics> GetStatistics([FromQuery]
            ConversationStatisticsParams conversationStatisticsParams
            )
        {
            var employerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var subscription = await _subscriptionRepository.GetSubscriptionForUserAsync(employerId);
            conversationStatisticsParams.SubscriptionId = subscription.Id;

            return await _conversationRepository.GetConversationsStatistics(conversationStatisticsParams);
        }
    }
}
