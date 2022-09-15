using Tgs.GameRecord.JDBService.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GameRecordRegisterExtensions
    {
        public static async Task<WebApplication> UseGameRecordRegister(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var sp = scope.ServiceProvider;
                var utensil = scope.ServiceProvider.GetRequiredService<IUtensil>();
                _ = await utensil.GetGameProviderIdAsync().ConfigureAwait(false);
            }

            return app;
        }
    }
}
