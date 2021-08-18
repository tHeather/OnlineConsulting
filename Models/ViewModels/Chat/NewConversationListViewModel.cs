using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ViewModels.Modals;
using OnlineConsulting.Tools;

namespace OnlineConsulting.Models.ViewModels.Chat
{
    public class NewConversationListViewModel
    {
        public PaginatedList<Conversation> ConversationList { get; set; }
        public ModalViewModel Modal { get; set; }
    }
}
