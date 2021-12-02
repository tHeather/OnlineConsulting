using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OnlineConsulting.Constants;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Models.ViewModels.Chat;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatHub(IConversationRepository conversationRepository, IChatMessageRepository chatMessageRepository)
        {
            _conversationRepository = conversationRepository;
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task CreateConversationAsync(CreateConversationViewModel createConversationViewModel)
        {
            var createConversation = new CreateConversation
            {
                Host = createConversationViewModel.Host,
                Path = createConversationViewModel.Path,
                SubscriptionId = createConversationViewModel.SubscriptionId
            };

            var conversation = await _conversationRepository.CreateConversationAsync(createConversation);

            var createMessage = new CreateMessage
            {
                Content = createConversationViewModel.FirstMessage,
                ConversationId = conversation.Id,
                IsFromClient = true
            };

            await _chatMessageRepository.CreateMessageAsync(createMessage);
            await JoinTheGroupAsync(conversation.Id.ToString());
        }

        public async Task JoinTheGroupAsync(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

            var conversationIdGuid = Guid.Parse(conversationId);
            var messages = await _chatMessageRepository.GetAllMessagesForConversationById(conversationIdGuid);

            var chatMessages = messages.Select(message => new ChatMessageViewModel
            {
                Content = message.Content,
                CreateDate = message.CreateDate.ToString(),
                IsFromClient = message.IsFromClient
            });

            var joinToConversationViewModel = new JoinToConversationViewModel
            {
                Messages = chatMessages,
                ConversationId = conversationId,
            };

            await Clients.Client(Context.ConnectionId)
                         .SendAsync("JoinedTheGroup", joinToConversationViewModel);
        }

        public async Task SendMessageAsync(string message, string conversationId)
        {
            var conversationIdGuid = Guid.Parse(conversationId);
            var conversation = await _conversationRepository.GetConversationByIdAsync(conversationIdGuid);

            if (conversation.Status == ConversationStatus.DONE)
            {
                await Clients.Group(conversationId).SendAsync("OnCloseConversationAsync");
                return;
            }

            var isMessageFromClient = !Context.User.Identity.IsAuthenticated;

            var createMessage = new CreateMessage
            {
                Content = message,
                ConversationId = conversation.Id,
                IsFromClient = isMessageFromClient
            };

            var savedMessage = await _chatMessageRepository.CreateMessageAsync(createMessage);

            var chatMessageViewModel = new ChatMessageViewModel
            {
                Content = savedMessage.Content,
                CreateDate = savedMessage.CreateDate.ToString(),
                IsFromClient = savedMessage.IsFromClient
            };

            await Clients.Group(conversationId).SendAsync("ReceiveMessageAsync", chatMessageViewModel);
        }

        [Authorize(Roles = UserRoleValue.CONSULTANT)]
        public async Task CloseConverationAsync(string conversationId)
        {
            var conversationIdGuid = Guid.Parse(conversationId);

            var conversation = await _conversationRepository.GetConversationByIdAsync(conversationIdGuid);
            await _conversationRepository.CloseConversationAsync(conversation);

            await Clients.Group(conversationId).SendAsync("OnCloseConversationAsync");
        }

    }
}