namespace FinantsApp;

using FinantsApp.Data;
using FinantsApp.Models;
using FinantsApp.ViewModels;
using System.Collections.ObjectModel;

public partial class SavingGoalPage : ContentPage
{
	private readonly DatabaseContext _db;
    private readonly TransactionsViewModel _transVm;

    public SavingGoal? CurrentGoal { get; set; }
    public ObservableCollection<SavingGoal> PreviousGoals { get; set; } = [];

    public bool ProgressNegative { get; set; } = false;


    public SavingGoalPage(DatabaseContext vm, TransactionsViewModel transVm)
	{
		InitializeComponent();
		_db = vm;
        _transVm = transVm;
		BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _transVm.LoadAsync();
        await ReloadEverything();
    }

    protected async Task ReloadEverything()
    {
        CurrentGoal = null;
        PreviousGoals = [];

        IEnumerable<SavingGoal> list = await _db.GetAllAsync<SavingGoal>();
        foreach (var item in list)
        {
            if (await IsActive(item))
            {
                CurrentGoal = item;
                continue;
            }

            PreviousGoals.Add(item);
        }

        UpdateCurrentGoalDisplay();
    }

    protected async Task<bool> IsActive(SavingGoal goal)
    {
        if (goal.State != GoalState.Active)
        {
            return false;
        }

        var date = DateTime.Now;
        var end = goal.CreationTime.AddDays(goal.DurationInDays);

        if (date > end)
        {
            goal.State = GoalState.Expired;
            await _db.UpdateItemAsync(goal);

            return false;
        }

        return true;
    }

    private void UpdateCurrentGoalDisplay()
    {
        if (CurrentGoal == null)
        {
            CurrentGoalSection.IsVisible = false;
        }
        else
        {
            var prog = _transVm.IncomeDifference - CurrentGoal.BalanceAtCreation;
            var barProg = Math.Min(1, prog / CurrentGoal.Goal);

            CurrentGoalLabel.Text = CurrentGoal.Name;
            CurrentGoalProgress.Text = $"{prog:C2}";
            CurrentGoalTarget.Text = $"{CurrentGoal.Goal:C2}";
            GoalProg.Progress = (double) barProg;

            ProgressNegative = prog < 0;

            if (ProgressNegative)
            {
                CurrentGoalProgress.TextColor = Colors.Red;
            } 
            else
            {
                CurrentGoalProgress.TextColor = Colors.Green;
            }

            CurrentGoalSection.IsVisible = true;
        }

        NewGoalFrame.IsVisible = !CurrentGoalSection.IsVisible;
        PreviousGoalsList.ItemsSource = PreviousGoals;
    }

    private async void OnNewGoalClicked(object sender, EventArgs e)
    {
        var goal = CreateSavingGoalFromForm();
        if (goal == null)
        {
            return;
        }

        goal.CreationTime = DateTime.Now;
        goal.BalanceAtCreation = _transVm.IncomeDifference;
        goal.State = GoalState.Active;

        await _db.AddItemAsync(goal);
        await ReloadEverything();

        In_GoalAmount.Text = "";
        In_GoalLength.Text = "";
        In_GoalName.Text = "";
    }

    private SavingGoal? CreateSavingGoalFromForm()
    {
        // Ensure inputs are valid before parsing
        if (string.IsNullOrWhiteSpace(In_GoalName.Text) ||
            string.IsNullOrWhiteSpace(In_GoalLength.Text) ||
            string.IsNullOrWhiteSpace(In_GoalAmount.Text))
        {
            DisplayAlert("Error", "Please fill in all fields.", "OK");
            return null;
        }

        // Try to parse GoalLength and GoalAmount as they are numeric inputs
        bool isDurationValid = int.TryParse(In_GoalLength.Text, out int goalDuration);
        bool isAmountValid = int.TryParse(In_GoalAmount.Text, out int goalAmount);

        if (!isDurationValid || !isAmountValid)
        {
            DisplayAlert("Error", "Please enter valid numeric values for duration and amount.", "OK");
            return null;
        }

        // Create and return the SavingGoal object
        var newGoal = new SavingGoal
        {
            Name = In_GoalName.Text,
            DurationInDays = goalDuration,
            Goal = goalAmount
        };

        return newGoal;
    }

    private async void OnRemoveGoalClicked(object sender, EventArgs e)
    {
        if (CurrentGoal == null)
        {
            return;
        }

        CurrentGoal.State = GoalState.Deactivated;
        await _db.UpdateItemAsync<SavingGoal>(CurrentGoal);

        await ReloadEverything();
    }
}