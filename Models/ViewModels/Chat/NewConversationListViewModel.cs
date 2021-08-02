using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Tools;

namespace OnlineConsulting.Models.ViewModels.Chat
{
    public class NewConversationListViewModel
    {
        public PaginatedList<NewConversationWithConnection> ConversationList { get; set; }
    }
}
