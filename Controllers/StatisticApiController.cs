using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Attributes;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.ValueObjects.Statistic;
using OnlineConsulting.Services.Repositories.Interfaces;
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

        public StatisticApiController(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }


        [HttpGet("get-statistics")]
        public async Task<ConversationStatistics> GetStatistics([FromQuery]
            ConversationStatisticsParams conversationStatisticsParams
            )
        {
            return await _conversationRepository.GetConversationsStatistics(conversationStatisticsParams);
        }
    }
}
