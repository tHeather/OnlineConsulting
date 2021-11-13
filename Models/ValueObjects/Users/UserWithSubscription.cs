using OnlineConsulting.Models.Entities;

namespace OnlineConsulting.Models.ValueObjects.Users
{
    public class UserWithSubscription
    {
        public User User { get; set; }
        public Subscription Subscription { get; set; }
    }
}
