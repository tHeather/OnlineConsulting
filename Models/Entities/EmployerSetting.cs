using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.Entities
{
    public class EmployerSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SubscriptionEndDate { get; set; }


        [StringLength(260, MinimumLength = 1)]
        public string Domain { get; set; }



        public User User { get; set; }

    }
}
