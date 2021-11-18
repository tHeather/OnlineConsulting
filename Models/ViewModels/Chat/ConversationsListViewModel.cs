using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ValueObjects.Chat;
using OnlineConsulting.Tools;

namespace OnlineConsulting.Models.ViewModels.Chat
{
    public class ConversationsListViewModel
    {
        public PaginatedList<Conversation> Conversations { get; set; }
        public ConversationFilters Filters { get; set; }
        public bool IsAscending { get; set; }
    }
}
