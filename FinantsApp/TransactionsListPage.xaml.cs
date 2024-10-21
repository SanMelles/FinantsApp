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

    private async void OnTransactionSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count < 1)
        {
            return;
        }

        Transaction selected = (Transaction) e.CurrentSelection[0];

        var services = Application.Current.MainPage.Handler.MauiContext.Services;
        var vm = services.GetRequiredService<TransactionsViewModel>();

        var page = new AddTransactionPage(vm, selected);

        await Navigation.PushAsync(page);
    }
}