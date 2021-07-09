using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.ViewModels.Modals
{
    public class ConfirmationModalViewModel
    {
        public string Id { get; set; }
        public string ModalLabel { get; set; }
        public string ModalText { get; set; }

        public string ConfirmController { get; set; }

        public string ConfirmAction { get; set; }

        public Dictionary<string,string> ConfirmParams { get; set; }

        public string ConfrimText { get; set; }

        public string  RejectText { get; set; }
    }
}
