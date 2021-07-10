using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.ViewModels.Modals
{
    public class ModalViewModel
    {

        public string ModalId { get; set; }

        public string ModalLabel { get; set; }

        public List<string> ModalText { get; set; }

        public string ModalType { get; set; }

        public bool IsVisible { get; set; }

    }
}
