﻿using Microsoft.AspNetCore.SignalR;
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
        private readonly IChatRepository _chatRepository;

        public ChatHub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task CreateConversationAsync(string firstMessage)
        {
            var createConversation = new CreateConversation
            {
                Host = Context.GetHttpContext().Request.Host.ToString(),
                Path = Context.GetHttpContext().Request.Path.ToString()
            };

            var conversation = await _chatRepository.CreateConversationAsync(createConversation);

            var createMessage = new CreateMessage
            {
                Content = firstMessage,
                Conversation = conversation,
                IsFromClient = true
            };

            await _chatRepository.CreateMessageAsync(createMessage);
            await JoinTheGroupAsync(conversation.Id.ToString());
        }

        public async Task JoinTheGroupAsync(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

            var conversationIdGuid = Guid.Parse(conversationId);
            var messages = await _chatRepository.GetAllMessagesForConversationById(conversationIdGuid);

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
            var conversation = await _chatRepository.GetConversationByIdAsync(conversationIdGuid);

            var isMessageFromClient = !Context.User.Identity.IsAuthenticated;

            var createMessage = new CreateMessage
            {
                Content = message,
                Conversation = conversation,
                IsFromClient = isMessageFromClient
            };

            var savedMessage = await _chatRepository.CreateMessageAsync(createMessage);

            var chatMessageViewModel = new ChatMessageViewModel
            {
                Content = savedMessage.Content,
                CreateDate = savedMessage.CreateDate.ToString(),
                IsFromClient = savedMessage.IsFromClient
            };

            await Clients.Group(conversationId).SendAsync("ReceiveMessageAsync", chatMessageViewModel);
        }
    }
}