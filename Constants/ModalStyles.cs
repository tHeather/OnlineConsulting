using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.Constants
{
    public static class ModalStyles
    {

        public const string INFO = "info";
        public const string ERROR = "error";
        public const string SUCCESS = "success";

        public static readonly ReadOnlyDictionary<string, string> CLASSES = new ReadOnlyDictionary<string, string>(
             new Dictionary<string, string>()
             {
                { $"{ERROR}Header", "bg-danger" },
                { $"{ERROR}Background", "alert-danger" },
                { $"{ERROR}Btn", "btn-danger" },

                { $"{INFO}Header", "bg-info" },
                { $"{INFO}Background", "" },
                { $"{INFO}Btn", "btn-info" },

                { $"{SUCCESS}Header", "bg-success "},
                { $"{SUCCESS}Background", "alert-success" },
                { $"{SUCCESS}Btn", "btn-success" }

             }
         );
    }
}
