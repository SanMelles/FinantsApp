namespace FinantsApp;

using FinantsApp.Models;
using FinantsApp.ViewModels;

public partial class AddTransactionPage : ContentPage
{
	private readonly TransactionsViewModel _vm;
	private readonly Transaction? _trans;

	public AddTransactionPage(TransactionsViewModel vm)
	{
		InitializeComponent();
		BindingContext = new AddViewModel();

		_vm = vm;
		_trans = null;
	}
	
	public AddTransactionPage(TransactionsViewModel vm, Transaction transaction)
	{
		InitializeComponent();
		BindingContext = new AddViewModel();

		_vm = vm;
		_trans = transaction;

		Title = "Update Transaction";

		ReasonPicker.SelectedItem = transaction.Reason;
		DescriptionEditor.Text = transaction.Description;
		AmountEntry.Text = transaction.Amount.ToString();
	}

	async void OnSubmitClicked(object sender, EventArgs e)
	{
		if (ReasonPicker.SelectedIndex == -1)
		{
			DisplayAlert("Error", "No transaction reason picked!", "OK");
			return;
		}

		TransactionReason? reasonStr = (TransactionReason?) ReasonPicker.SelectedItem;
		var desc = DescriptionEditor.Text;
		var amountStr = AmountEntry.Text;

		if (!decimal.TryParse(amountStr, out decimal amount))
		{
			DisplayAlert("Error", "Please enter a valid amount.", "OK");
			return;
		}
		if (reasonStr == null)
		{
			DisplayAlert("Error", "Please enter a transaction reason!", "OK");
			return;
		}

		Transaction transaction = new()
		{
			Amount = amount,
			Reason = (TransactionReason) reasonStr,
			Description = desc,
			Date = DateTime.Now,
		};

		if (_trans == null)
		{
            await _vm.AddTransaction(transaction);
            DisplayAlert("Success", "Transaction submitted", "OK");
        }
		else
		{
			transaction.Id = _trans.Id;
            await _vm.UpdateTransaction(transaction);
            DisplayAlert("Success", "Transaction updated", "OK");
        }

        ReasonPicker.SelectedItem = null;
        DescriptionEditor.Text = null;
        AmountEntry.Text = null;

		await Navigation.PopAsync();
	}

	private async void OnDeleteTransaction(object sender, EventArgs e)
	{
		if (_trans == null)
		{
			DisplayAlert("Error", "How is this even visible??", "OK");
			return;
		}


	}
}

public partial class AddViewModel
{
	public IEnumerable<TransactionReason> Reasons { get; set; } = Enum.GetValues(typeof(TransactionReason)).Cast<TransactionReason>().ToList();
}