namespace FinantsApp.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

public class TransactionGroup
{
    public ObservableCollection<Transaction> Transactions { get; set; } = [];
    public int Year { get; set; }
    public int Month { get; set; }

    public bool IsCurrentMonth => DateTime.Now.Month == Month;

    public string DisplayLabel
    {
        get
        {
            if (IsCurrentMonth)
            {
                return "Current Month";
            }

            return $"{Year} {MonthName}";
        }
    }

    public string MonthName
    {
        get
        {
            var ci = CultureInfo.CurrentCulture;
            return ci.TextInfo.ToTitleCase(ci.DateTimeFormat.GetMonthName(Month));
        }
    }
}
