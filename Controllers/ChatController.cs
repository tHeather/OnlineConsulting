using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineConsulting.Constants;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Models.ViewModels.Chat;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System.Linq;
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
        [HttpGet("new-conversation-list")]
        public async Task<IActionResult> NewConversationList(int pageIndex = 1)
        {
            var connections = await _chatRepository.GetAllConnectionsAsync();
            var newConversations = _chatRepository.GetNewConversationsQuery();

            var newConversationsPaginated = await PaginatedList<Conversation>
                                                            .CreateAsync(
                                                                newConversations,
                                                                pageIndex,
                                                                10);


            var newConversationWithConnections = newConversationsPaginated
                .Select(conversation =>
                    new NewConversationWithConnection
                    {
                        ConnectionId = connections.SingleOrDefault(
                            c => c.Value.ToString() == conversation.Id.ToString()
                            ).Name,
                        Conversation = conversation
                    }
                );


            var newConversationWithConnectionsPaginated = new PaginatedList<NewConversationWithConnection>(
                                                                                newConversationWithConnections,
                                                                                newConversationsPaginated.TotalPages,
                                                                                newConversationsPaginated.PageIndex);

            return View(
                "NewConversationList",
                    new NewConversationListViewModel
                    {
                        ConversationList = newConversationWithConnectionsPaginated
                    }
                );
        }

    }
}
