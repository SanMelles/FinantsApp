namespace FinantsApp.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class TransactionSummaryInfo
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }

    public decimal Difference => Income - Expenses;

    public bool IsPositive => Income >= Expenses;

    public string MonthName
    {
        get
        {
            var ci = CultureInfo.CurrentCulture;
            return ci.TextInfo.ToTitleCase(ci.DateTimeFormat.GetMonthName(Month));
        }
    }

    public void Account(Transaction transaction)
    {
        if (transaction.IsIncome)
        {
            Income += transaction.Amount;
        } 
        else
        {
            Expenses += transaction.Amount;
        }
    }
}
