namespace FinantsApp.ApplicationServices;

using FinantsApp.Models;
using FinantsApp.ServiceInterface;
using FinantsApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TransactionService : ITransactionService
{
    private readonly DatabaseContext _context;

    public TransactionService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Transaction> AddTransaction(Transaction transaction)
    {
        await _context.AddItemAsync<Transaction>(transaction);
        return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        return await _context.GetAllAsync<Transaction>();
    }

    public async Task<Transaction?> GetTransactionAsync(int? id)
    {
        if (id == null)
        {
            return null;
        }

        return await _context.GetItemByKeyAsync<Transaction>(id);
    }

    public async Task<bool> RemoveTransaction(int? id)
    {
        if (id == null)
        {
            return false;
        }

        return await _context.DeleteItemByKeyAsync<Transaction>(id);
    }

    public async Task<Transaction> UpdateTransaction(Transaction transaction)
    {
        await _context.UpdateItemAsync<Transaction>(transaction);
        return transaction;
    }
}
