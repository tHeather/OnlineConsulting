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
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            var conversation = await _chatRepository.GetConversationByIdAsync(consultantChatConnection.ConversationId);
            var consultantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (
                conversation.Status == ConversationStatus.IN_PROGRESS &&
                conversation.ConsultantId == consultantId
                ) return View("ConsultantChat", new ConsultantChatViewModel
                {
                    ConversationId = consultantChatConnection.ConversationId,
                    RedirectAction = consultantChatConnection.RedirectAction
                });

            if (conversation.Status != ConversationStatus.NEW) return RedirectToAction("NewConversationList", new ModalViewModel
            {
                ModalLabel = "Already taken",
                ModalText = new List<string>() { "Other consultant has already joined to this conversation." },
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });

            var isSaved = await _chatRepository.AssignConsultantToConversation(conversation,
                                                                             consultantId,
                                                                             consultantChatConnection.RowVersion);

            if (!isSaved) return RedirectToAction("NewConversationList", new ModalViewModel
            {
                ModalLabel = "Already taken",
                ModalText = new List<string>() { "Other consultant has already joined to this conversation." },
                IsVisible = true,
                ModalType = ModalStyles.ERROR
            });


            return View("ConsultantChat", new ConsultantChatViewModel
            {
                ConversationId = consultantChatConnection.ConversationId,
                RedirectAction = consultantChatConnection.RedirectAction
            });
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("new-conversation-list")]
        public async Task<IActionResult> NewConversationList(int pageIndex = 1, ModalViewModel modalViewModel = null)
        {
            var newConversations = _chatRepository.GetNewConversationsQuery();

            var newConversationsPaginated = await PaginatedList<Conversation>.CreateAsync(
                                                                                newConversations,
                                                                                pageIndex,
                                                                                10);

            return View(
                "NewConversationList",
                    new NewConversationListViewModel
                    {
                        ConversationList = newConversationsPaginated,
                        Modal = modalViewModel
                    }
                );
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("in-progress-conversation-list")]
        public async Task<IActionResult> InProgressConversationList(int pageIndex = 1)
        {
            var consultantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var conversationsInProgress = _chatRepository.GetInProgressConversationsForConsultantQuery(consultantId);

            var conversationsInProgressPaginated = await PaginatedList<Conversation>.CreateAsync(
                                                                                conversationsInProgress,
                                                                                pageIndex,
                                                                                10);

            return View("InProgressConversationList", conversationsInProgressPaginated);
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpPost("change-conversation-status")]
        public async Task<IActionResult> ChangeConversationStatus(
            Guid conversationId, ConversationStatus conversationStatus, string redirectAction)
        {
            var conversation = await _chatRepository.GetConversationByIdAsync(conversationId);
            await _chatRepository.ChangeConversationStatusAsync(conversation, conversationStatus);

            return RedirectToAction(redirectAction);
        }

    }
}
