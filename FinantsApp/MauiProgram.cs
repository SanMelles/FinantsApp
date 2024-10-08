using FinantsApp.Data;
using FinantsApp.ServiceInterface;
using FinantsApp.ApplicationServices;
using Microsoft.Extensions.Logging;

namespace FinantsApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            InitializeDatabase(app.Services.GetRequiredService<ITransactionService>()).Wait();
            return app;
        }

        // Add dummy data to databse if it's empty.
        private static async Task InitializeDatabase(ITransactionService service)
        {
            var all = await service.GetAllTransactionsAsync();

            if (all.Any())
            {
                return;
            }


        }
    }
}
