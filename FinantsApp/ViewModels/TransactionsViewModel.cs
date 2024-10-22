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

        [ObservableProperty]
        private ObservableCollection<Transaction> _transactions = [];

        [ObservableProperty]
        private ObservableCollection<Transaction> _sortedTransactions = [];

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
            IEnumerable<Transaction> res = await _service.GetAllAsync<Transaction>();
            
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

            SortedTransactions = new(Transactions.OrderByDescending(x => x.Date));

            IncomeDifference = TotalIncome - TotalExpenses;
            IncomeGreaterThanExpenses = IncomeDifference > 0;
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
