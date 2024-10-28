using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using FinantsApp.Models;
using FinantsApp.Data;

namespace FinantsApp.ViewModels
{
    public partial class TransactionsViewModel: ObservableObject
    {
        private readonly DatabaseContext _service;

        public TransactionsViewModel(DatabaseContext service)
        {
            _service = service;
        }

        // This is a mess lmao

        [ObservableProperty]
        private ObservableCollection<Transaction> _transactions = [];

        [ObservableProperty]
        private ObservableCollection<Transaction> _sortedTransactions = [];

        [ObservableProperty]
        private ObservableCollection<Transaction> _filteredTransactions = [];

        [ObservableProperty]
        private decimal _totalIncome = 0;

        [ObservableProperty]
        private decimal _totalExpenses = 0;

        [ObservableProperty]
        private decimal _incomeDifference = 0;

        [ObservableProperty]
        private bool _incomeGreaterThanExpenses = false;

        [ObservableProperty]
        private ObservableCollection<TransactionSummaryInfo> _summariesByTime = [];

        public async Task LoadAsync()
        {
            IEnumerable<Transaction> res = await _service.GetAllAsync<Transaction>();
            
            if (Transactions == null)
            {
                Transactions = [];
            } 
            else
            {
                Transactions.Clear();
            }

            SummariesByTime.Clear();

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

                OnTransactionLoaded(transaction);
            }

            SortedTransactions = new(Transactions.OrderByDescending(x => x.Date));
            SummariesByTime = new(SummariesByTime.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month));

            IncomeDifference = TotalIncome - TotalExpenses;
            IncomeGreaterThanExpenses = IncomeDifference > 0;

            FilteredTransactions.Clear();
            int m = Math.Min(3, SortedTransactions.Count);
            for (int i = 0 ; i < m; i++)
            {
                var transaction = SortedTransactions[i];
                FilteredTransactions.Add(transaction);
            }
        }

        private void OnTransactionLoaded(Transaction transaction)
        {
            foreach (var item in SummariesByTime)
            {
                if (item.Year != transaction.Date.Year || item.Month != transaction.Date.Month)
                {
                    continue;
                }

                item.Account(transaction);
                return;
            }

            TransactionSummaryInfo summary = new()
            {
                Year = transaction.Date.Year,
                Month = transaction.Date.Month
            };
            summary.Account(transaction);

            SummariesByTime.Add(summary);
        }

        public async Task AddTransaction(Transaction transaction)
        {
            await _service.AddItemAsync(transaction);
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            await _service.UpdateItemAsync(transaction);
        }

        public async Task DeleteTransaction(Transaction transaction)
        {
            await _service.DeleteItemAsync(transaction);
        }
    }
}
