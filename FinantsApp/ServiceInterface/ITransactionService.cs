namespace FinantsApp.ServiceInterface;

using System;
using System.Collections.Generic;
using System.Text;
using FinantsApp.Models;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();

    Task<Transaction?> GetTransactionAsync(int? id);

    Task<Transaction> AddTransaction(Transaction transaction);

    Task<Transaction> UpdateTransaction(Transaction transaction);

    Task<bool> RemoveTransaction(int? id);
}
