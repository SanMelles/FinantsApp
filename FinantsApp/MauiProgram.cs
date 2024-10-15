using FinantsApp.Data;
using FinantsApp.Models;
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
                var service = app.Services.GetRequiredService<DatabaseContext>();
                await InitializeDatabase(service);
            });

            return app;
        }

        // Add dummy data to databse if it's empty.
        private static async Task InitializeDatabase(DatabaseContext service)
        {
            var all = await service.GetAllAsync<Transaction>();

            if (all.Any())
            {
                return;
            }

            Transaction[] transactions =
            {            
                new() { 
                    Amount = 2500.00m, 
                    Reason = TransactionReason.Salary, 
                    Description = "Monthly salary", 
                    Date = new DateTime(2024, 1, 1) 
                },
                new() { 
                    Amount = 200.00m, 
                    Reason = TransactionReason.Bonus, 
                    Description = "Year-end bonus", 
                    Date = new DateTime(2024, 1, 5) 
                },
                new() { 
                    Amount = 50.00m, 
                    Reason = TransactionReason.Gift, 
                    Description = "Birthday gift", 
                    Date = new DateTime(2024, 2, 10) 
                },
                new() { 
                    Amount = 150.00m, 
                    Reason = TransactionReason.InvestmentReturn, 
                    Description = "Stock dividend", 
                    Date = new DateTime(2024, 3, 15) 
                },
                new() { 
                    Amount = 800.00m, 
                    Reason = TransactionReason.LivingCosts, 
                    Description = "Rent payment", 
                    Date = new DateTime(2024, 3, 1) 
                },
                new() { 
                    Amount = 60.00m, 
                    Reason = TransactionReason.Entertainment, 
                    Description = "Movie tickets", 
                    Date = new DateTime(2024, 2, 20) 
                },
                new() { 
                    Amount = 30.00m, 
                    Reason = TransactionReason.Transportation, 
                    Description = "Taxi fare", 
                    Date = new DateTime(2024, 2, 22) 
                },
                new() { 
                    Amount = 120.00m, 
                    Reason = TransactionReason.LivingCosts, 
                    Description = "Groceries", 
                    Date = new DateTime(2024, 2, 5) 
                },
                new() { 
                    Amount = 2500.00m, 
                    Reason = TransactionReason.Salary, 
                    Description = "Monthly salary", 
                    Date = new DateTime(2024, 2, 1) 
                },
                new() { 
                    Amount = 180.00m, 
                    Reason = TransactionReason.InvestmentReturn, 
                    Description = "Real estate income", 
                    Date = new DateTime(2024, 4, 1) 
                },
                new() { 
                    Amount = 50.00m, 
                    Reason = TransactionReason.Entertainment, 
                    Description = "Concert tickets", 
                    Date = new DateTime(2024, 3, 28) 
                },
                new() { 
                    Amount = 15.00m, 
                    Reason = TransactionReason.Transportation, 
                    Description = "Bus fare", 
                    Date = new DateTime(2024, 3, 15) 
                },
                new() { 
                    Amount = 90.00m, 
                    Reason = TransactionReason.LivingCosts, 
                    Description = "Utility bill", 
                    Date = new DateTime(2024, 3, 10) 
                },
                new() { 
                    Amount = 2500.00m, 
                    Reason = TransactionReason.Salary, 
                    Description = "Monthly salary", 
                    Date = new DateTime(2024, 3, 1) 
                },
                new() { 
                    Amount = 500.00m, 
                    Reason = TransactionReason.Other, 
                    Description = "Freelance project", 
                    Date = new DateTime(2024, 4, 5) 
                },
                new() { 
                    Amount = 40.00m, 
                    Reason = TransactionReason.Entertainment, 
                    Description = "Dining out", 
                    Date = new DateTime(2024, 4, 8) 
                }
            };

            foreach (var item in transactions)
            {
                await service.AddItemAsync(item);
            }
        }
    }
}
