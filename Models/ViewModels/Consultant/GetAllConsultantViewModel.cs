using OnlineConsulting.Models.Entities;
using OnlineConsulting.Models.ViewModels.Modals;
using OnlineConsulting.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.ViewModels.Consultant
{
    public class GetAllConsultantViewModel
    {
        public PaginatedList<User> ConsultantList { get; set; }

        public ModalViewModel Modal { get; set; }
    }
}
