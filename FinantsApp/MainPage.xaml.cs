using FinantsApp.ViewModels;

namespace FinantsApp
{
    public partial class MainPage : ContentPage
    {
        private TransactionsViewModel _vm;
        private readonly TransactionsListPage _listPage;
        private readonly TransactionsDifferencePage _diffPage;
        private readonly AddTransactionPage _addPage;
        private readonly SavingGoalPage _savingsPage;

        public MainPage(
            TransactionsViewModel vm,
            TransactionsListPage listPage,
            TransactionsDifferencePage differencePage, 
            AddTransactionPage addTransactionPage,
            SavingGoalPage savingsPage)
        {
            InitializeComponent();

            BindingContext = vm;

            _vm = vm;
            _listPage = listPage;
            _diffPage = differencePage;
            _addPage = addTransactionPage;
            _savingsPage = savingsPage;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.LoadAsync();

            // Only keep the last 3 entries
            while (_vm.SortedTransactions.Count > 3)
            {
                _vm.SortedTransactions.RemoveAt(_vm.SortedTransactions.Count - 1);
            }
        }


        async void OnListOpen(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_listPage);
        }

        async void OnDifferenceOpen(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_diffPage);
        }

        async void OnAddNewTransaction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_addPage);
        }

        async void OnShowSavingGoal(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_savingsPage);
        }
    }

}
