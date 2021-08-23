using System.Collections.Generic;

namespace OnlineConsulting.Models.ViewModels.Chat
{
    public class JoinToConversationViewModel
    {
        public string ConversationId { get; set; }
        public IEnumerable<ChatMessageViewModel> Messages { get; set; }
    }
}
