using Hangfire.Dashboard;

namespace Tgs.GameRecord.JDB;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
