namespace FinantsApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly TransactionsListPage _listPage;
        private readonly TransactionsDifferencePage _diffPage;

        public MainPage(TransactionsListPage listPage, TransactionsDifferencePage differencePage)
        {
            InitializeComponent();
            _listPage = listPage;
            _diffPage = differencePage;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        async void OnListOpen(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_listPage);
        }

        async void OnDifferenceOpen(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_diffPage);
        }
    }

}
