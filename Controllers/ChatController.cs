using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineConsulting.Attributes;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineConsulting.Controllers
{
    [ValidateAntiForgeryToken]
    [TypeFilter(typeof(ValidateSubscriptionAttribute))]
    [Route("chat")]
    public class ChatController : Controller
    {
        const int PAGE_SIZE = 10;
        private readonly IConversationRepository _conversationRepository;
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatController(
            IConversationRepository conversationRepository, 
            IConfiguration configuration,
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository,
            IChatMessageRepository chatMessageRepository
            )
        {
            _conversationRepository = conversationRepository;
            _configuration = configuration;
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
            _chatMessageRepository = chatMessageRepository;
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.EMPLOYER)]
        [HttpGet("get-snippet")]
        public async Task<IActionResult> GetSnippet()
        {
            var origin = _configuration[Parameters.APPLICATION_URL] ??
                                    throw new ArgumentNullException(Parameters.APPLICATION_URL);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var subscription = await _subscriptionRepository.GetSubscriptionForUserAsync(userId);

            return View(new GetSnippetViewModel()
            {
                SubscriptionId = subscription.Id,
                Origin = origin
            });
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpPost("consultant")]
        public async Task<IActionResult> ConsultantChat(ConsultantChatConnection consultantChatConnection)
        {
            var conversation = await _conversationRepository.GetConversationByIdAsync(consultantChatConnection.ConversationId);
            var consultantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (
                conversation.Status == ConversationStatus.IN_PROGRESS &&
                conversation.ConsultantId == consultantId
                )
            {
                return View("ConsultantChat", new ConsultantChatViewModel
                {
                    ConversationId = consultantChatConnection.ConversationId,
                    RedirectAction = consultantChatConnection.RedirectAction,
                    Host = conversation.Host,
                    Path = conversation.Path
                });
            }

            if (conversation.Status != ConversationStatus.NEW)
            {
                return RedirectToAction("NewConversationList", new ModalViewModel
                {
                    ModalLabel = "Already taken",
                    ModalText = new List<string>() { "Other consultant has already joined to this conversation." },
                    IsVisible = true,
                    ModalType = ModalStyles.ERROR
                });
            }

            var isSaved = await _conversationRepository.AssignConsultantToConversation(conversation,
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
                RedirectAction = consultantChatConnection.RedirectAction,
                Host = conversation.Host,
                Path = conversation.Path
            });
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("new-conversation-list")]
        public async Task<IActionResult> NewConversationList(
            int pageIndex = 1, ModalViewModel modalViewModel = null)
        {
            var conversationsQuery = _conversationRepository.GetNewConversationsQuery();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var employer = _userRepository.GetEmployerForConsultant(userId);

            conversationsQuery = await _conversationRepository
                                           .GetConversationsForRoleQuery(employer.Id, conversationsQuery);

            var newConversationsPaginated = await PaginatedList<Conversation>
                                                    .CreateAsync(conversationsQuery, pageIndex,
                                                                 PAGE_SIZE);

            return View(
                "NewConversationList",
                    new NewConversationListViewModel
                    {
                        ConversationList = newConversationsPaginated,
                        Modal = modalViewModel
                    }
                );
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        [HttpGet("in-progress-conversation-list")]
        public async Task<IActionResult> InProgressConversationList(int pageIndex = 1)
        {
            var consultantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var conversationsInProgress = _conversationRepository
                                                .GetInProgressConversationsForConsultantQuery(consultantId);

            var conversationsInProgressPaginated = await PaginatedList<Conversation>
                                                                       .CreateAsync(conversationsInProgress,
                                                                                    pageIndex, PAGE_SIZE);

            return View("InProgressConversationList", conversationsInProgressPaginated);
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.CONSULTANT + "," + UserRoleValue.EMPLOYER + "," + UserRoleValue.ADMIN)]
        [HttpGet("conversation-list")]
        public async Task<IActionResult> ConversationList(
            ConversationFilters filters, int pageIndex = 1, bool isAscending = false)
        {
            var conversationsQuery = _conversationRepository
                                          .GetFilteredAndSortedConversationsQuery(filters, isAscending);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            conversationsQuery = await _conversationRepository
                                           .GetConversationsForRoleQuery(userId, conversationsQuery);

            var conversationsPaginated = await PaginatedList<Conversation>
                                            .CreateAsync(conversationsQuery, pageIndex, PAGE_SIZE);

            return View(new ConversationsListViewModel() {
                Conversations = conversationsPaginated,
                Filters = filters,
                IsAscending = isAscending,
                UserRole = _userRepository.GetUserRole(userId)
            });
        }

        [IgnoreAntiforgeryToken]
        [Authorize(Roles = UserRoleValue.CONSULTANT + "," + UserRoleValue.EMPLOYER + "," + UserRoleValue.ADMIN)]
        [HttpGet("messages")]
        public async Task<IActionResult> ConversationMessages(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var conversationQuery = await _conversationRepository.GetConversationsForRoleQuery(userId);
            var conversation = conversationQuery.SingleOrDefault(c => c.Id == id);

            IEnumerable<ChatMessage> messages = null;

            if (conversation != null)
            {
                messages = await _chatMessageRepository.GetAllMessagesForConversationById(id);
            }
           
            return View(new ConversationMessagesViewModel()
            {
                Conversation = conversation,
                Messages = messages
            });
        }

    }
}
