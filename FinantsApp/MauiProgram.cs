using FinantsApp.Data;
using FinantsApp.Models;
using FinantsApp.ServiceInterface;
using FinantsApp.ApplicationServices;
using FinantsApp.ViewModels;
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
            builder.Services.AddSingleton<TransactionsViewModel>();
            builder.Services.AddSingleton<TransactionsListPage>();
            builder.Services.AddSingleton<TransactionsDifferencePage>();
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var service = app.Services.GetRequiredService<ITransactionService>();
                await InitializeDatabase(service);
            });

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

            Transaction[] transactions =
            {
                new()
                {
                    Amount = 10.0m,
                    Reason = TransactionReason.Entertainment,
                    Description = "Cinema Ticket",
                    Date = DateTime.Now
                },
                new()
                {
                    Amount = 1234.5m,
                    Reason = TransactionReason.Salary,
                    Description = "Job at big mega corp",
                    Date = DateTime.Now
                },
                new()
                {
                    Amount = 377.47m,
                    Reason = TransactionReason.LivingCosts,
                    Description = "Rent",
                    Date = DateTime.Now
                }
            };

            foreach (var item in transactions)
            {
                await service.AddTransaction(item);
            }
        }
    }
}
