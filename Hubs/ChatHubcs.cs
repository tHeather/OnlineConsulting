using Microsoft.AspNetCore.SignalR;
using OnlineConsulting.Enums;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
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

        public override async Task OnConnectedAsync()
        {

            if (!Context.User.Identity.IsAuthenticated)
            {

                var createConversation = new CreateConversation
                {
                    Host = Context.GetHttpContext().Request.Host.ToString(),
                    Path = Context.GetHttpContext().Request.Path.ToString()
                };

                var conversation = await _chatRepository.CreateConversationAsync(createConversation);
                await Groups.AddToGroupAsync(Context.ConnectionId, Context.ConnectionId);
                await _chatRepository.AddConnectionAsync(Context.ConnectionId, conversation.Id.ToString());
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            if (!Context.User.Identity.IsAuthenticated)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.ConnectionId);
                var conversation = await _chatRepository.GetConversationByConnectionIdAsync(Context.ConnectionId);
                await _chatRepository.ChangeConversationStatusAsync(conversation, ConversationStatus.DONE);
                await _chatRepository.RemoveConnectionAsync(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinToGroupAsync(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public async Task SendMessageAsync(string origin, string message, string clientConnectionId)
        {
            var connectionId = clientConnectionId ?? Context.ConnectionId;

            var conversation = await _chatRepository.GetConversationByConnectionIdAsync(connectionId);

            var createMessage = new CreateMessage
            {
                Content = message,
                Conversation = conversation
            };

            await _chatRepository.CreateMessageAsync(createMessage);

            await Clients.Group(connectionId).SendAsync("ReceiveMessageAsync", message);
        }
    }
}