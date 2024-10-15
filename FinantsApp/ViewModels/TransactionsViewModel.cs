using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using FinantsApp.ServiceInterface;
using FinantsApp.Models;

namespace FinantsApp.ViewModels
{
    public partial class TransactionsViewModel: ObservableObject
    {
        private readonly ITransactionService _service;

        public TransactionsViewModel(ITransactionService service)
        {
            _service = service;
        }

        [ObservableProperty]
        private ObservableCollection<Transaction> _transactions = [];

        [ObservableProperty]
        private decimal _totalIncome = 0;

        [ObservableProperty]
        private decimal _totalExpenses = 0;

        [ObservableProperty]
        private decimal _incomeDifference = 0;

        [ObservableProperty]
        private bool _incomeGreaterThanExpenses = false;

        public async Task LoadAsync()
        {
            IEnumerable<Transaction> res = await _service.GetAllTransactionsAsync();
            
            if (Transactions == null)
            {
                Transactions = [];
            } 
            else
            {
                Transactions.Clear();
            }

            TotalIncome = 0;
            TotalExpenses = 0;

            foreach (var transaction in res)
            {
                Transactions.Add(transaction);

                if (transaction.IsIncome)
                {
                    TotalIncome += transaction.Amount;
                } 
                else
                {
                    TotalExpenses += transaction.Amount;
                }
            }

            IncomeDifference = TotalIncome - TotalExpenses;
            IncomeGreaterThanExpenses = IncomeDifference > 0;
        }
    }
}
