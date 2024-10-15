using FinantsApp.ViewModels;

namespace FinantsApp;

public partial class TransactionsDifferencePage : ContentPage
{
	private readonly TransactionsViewModel _viewModel;

	public TransactionsDifferencePage(TransactionsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}