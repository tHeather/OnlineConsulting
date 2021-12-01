using OnlineConsulting.Models.Entities;
using System.Collections.Generic;

namespace OnlineConsulting.Models.ViewModels.Chat
{
    public class ConversationMessagesViewModel
    {
        public IEnumerable<ChatMessage> Messages { get; set; }
        public Conversation Conversation { get; set; }
    }
}
