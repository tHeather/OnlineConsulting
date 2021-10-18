using OnlineConsulting.Models.Entities;
using System.Collections.Generic;

namespace OnlineConsulting.Models.ViewModels.Subscription
{
    public class ChangeSubscriptionPriceViewModel
    {
        public SubscriptionType SubscriptionToUpdate { get; set; }
        public bool IsSaved { get; set; }
        public List<SubscriptionType> SubscriptionTypes { get; set; }
    }
}
