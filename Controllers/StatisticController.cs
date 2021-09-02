using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.ValueObjects.Statistic;
using OnlineConsulting.Services.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Authorize(Roles = UserRoleValue.EMPLOYER)]
    [Route("statistics")]
    public class StatisticController : Controller
    {

        private readonly IConversationRepository _conversationRepository;

        public StatisticController(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        [HttpGet("")]
        public IActionResult GetStatistics()
        {
            return View();
        }


        [HttpGet("/api/statistics/get-statistics")]
        public async Task<ConversationStatistics> GetStatisticsAPI(ConversationStatisticsParams conversationStatisticsParams)
        {

            var statistics = await _conversationRepository.GetConversationsStatistics(conversationStatisticsParams);

            return statistics;
        }
    }
}
