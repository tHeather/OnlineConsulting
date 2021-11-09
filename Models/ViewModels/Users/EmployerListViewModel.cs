using OnlineConsulting.Models.ValueObjects.Users;
using OnlineConsulting.Tools;

namespace OnlineConsulting.Models.ViewModels.Users
{
    public class EmployerListViewModel
    {
        public PaginatedList<UserWithSubscription> Employers { get; set; }

    }
}
