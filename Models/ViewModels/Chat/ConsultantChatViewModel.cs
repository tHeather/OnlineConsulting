using System;

namespace OnlineConsulting.Models.ViewModels.Chat
{
    public class ConsultantChatViewModel
    {
        public Guid ConversationId { get; set; }
        public string RedirectAction { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
    }
}
