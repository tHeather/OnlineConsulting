using OnlineConsulting.Models.Entities;
using OnlineConsulting.Tools;
using System.Collections.Generic;

namespace OnlineConsulting.Models.ViewModels.Users
{
    public class EmployeeListViewModel
    {
        public PaginatedList<User> Employees { get; set; }
        public User Employer { get; set; }
    }
}
