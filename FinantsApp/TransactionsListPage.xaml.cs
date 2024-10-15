namespace FinantsApp;

using FinantsApp.Models;
using FinantsApp.ViewModels;

public partial class TransactionsListPage : ContentPage
{

    private readonly TransactionsViewModel _service;

    public TransactionsListPage(TransactionsViewModel service)
    {
        InitializeComponent();
        _service = service;
        BindingContext = _service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _service.LoadAsync();
    }

    void OnTransactionSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count < 1)
        {
            return;
        }

        Transaction selected = (Transaction) e.CurrentSelection[0];
        // TODO: Open detail/edit or something page for this transaction
    }
}