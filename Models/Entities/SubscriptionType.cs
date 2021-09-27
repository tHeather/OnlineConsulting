using System;

namespace OnlineConsulting.Models.Entities
{
    public class SubscriptionType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
        public decimal Price { get; set; }
    }
}
