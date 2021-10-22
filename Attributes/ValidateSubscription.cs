using Microsoft.AspNetCore.Mvc.Filters;
using OnlineConsulting.Constants;
using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Security.Claims;

namespace OnlineConsulting.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateSubscriptionAttribute : ActionFilterAttribute
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;

        public ValidateSubscriptionAttribute(
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository
            )
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            if (role != UserRoleValue.ADMIN)
            {
                var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _userRepository.GetUserById(userId);
                var userWithSubscriptionId = user.EmployerId ?? user.Id;
                var subscription = await _subscriptionRepository.GetSubscriptionForUserAsync(userWithSubscriptionId);

                if (subscription.EndDate < DateTime.UtcNow)
                {
                    context.HttpContext.Response.Redirect($"/subscription/{role.ToLower()}/subscription-expired");
                };
            }

        }
    }
}


