using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Services.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Route("chat")]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public IActionResult Index()
        {
            return View("ClientChat");
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("consultant")]
        public IActionResult ConsultantChat(string clientConnectionId)
        {
            return View("ConsultantChat", clientConnectionId);
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("active-conversation-list")]
        public async Task<IActionResult> ActiveConversationList()
        {
            var connectionsHashEntry = await _chatRepository.GetAllConnectionsAsync();

            return View("ActiveConversationList", connectionsHashEntry);
        }

    }
}
