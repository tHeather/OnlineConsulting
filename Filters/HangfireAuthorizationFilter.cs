using Hangfire.Annotations;
using Hangfire.Dashboard;
using OnlineConsulting.Constants;

namespace OnlineConsulting.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole(UserRoleValue.ADMIN);
        }
    }
}
