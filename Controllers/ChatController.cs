﻿using Microsoft.AspNetCore.Authorization;
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
            var conversationId = Guid.Parse(consultantChatConnection.ConversationId);
            var conversation = await _chatRepository.GetConversationByIdAsync(conversationId);
            var consultantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (
                conversation.Status == ConversationStatus.IN_PROGRESS &&
                conversation.ConsultantId == consultantId
                ) return View("ConsultantChat", consultantChatConnection.ConversationId);

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


            return View("ConsultantChat", consultantChatConnection.ConversationId);
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

    }
}
