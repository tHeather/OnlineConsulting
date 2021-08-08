using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineConsulting.Constants;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.Constants;
using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Models.ViewModels.Chat;
using OnlineConsulting.Models.ViewModels.Modals;
using OnlineConsulting.Services.Repositories.Interfaces;
using OnlineConsulting.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [Route("chat")]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly IConfiguration _configuration;

        public ChatController(IChatRepository chatRepository, IConfiguration configuration)
        {
            _chatRepository = chatRepository;
            _configuration = configuration;
        }

        [Authorize(Roles = UserRoleValue.EMPLOYER)]
        [HttpGet("get-snippet")]
        public IActionResult GetSnippet()
        {
            return View(_configuration);
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpPost("consultant")]
        public async Task<IActionResult> ConsultantChat(ConsultantChatConnection consultantChatConnection)
        {

            var conversation = await _chatRepository.GetConversationByClientConnectionIdAsync(consultantChatConnection.ClientConnectionId);

            if (conversation == null) return RedirectToAction("NewConversationList", new ModalViewModel
            {
                ModalLabel = "Ended conversation",
                ModalText = new List<string>() { "The client has ended the conversation." },
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });

            if (conversation.Status != ConversationStatus.NEW) return RedirectToAction("NewConversationList", new ModalViewModel
            {
                ModalLabel = "Already taken",
                ModalText = new List<string>() { "Other consultant has already joined to this conversation." },
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });

            var isSaved = await _chatRepository.ChangeConversationStatusConcurrencySafeAsync(conversation,
                                                                             ConversationStatus.IN_PROGRESS,
                                                                             consultantChatConnection.RowVersion);

            if (!isSaved) return RedirectToAction("NewConversationList", new ModalViewModel
            {
                ModalLabel = "Already taken",
                ModalText = new List<string>() { "Other consultant has already joined to this conversation." },
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });


            return View("ConsultantChat", consultantChatConnection.ClientConnectionId);
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("new-conversation-list")]
        public async Task<IActionResult> NewConversationList(int pageIndex = 1, ModalViewModel modalViewModel = null)
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
                        Conversation = conversation,
                        RowVersion = conversation.RowVersion
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
                        ConversationList = newConversationWithConnectionsPaginated,
                        Modal = modalViewModel
                    }
                );
        }

    }
}
