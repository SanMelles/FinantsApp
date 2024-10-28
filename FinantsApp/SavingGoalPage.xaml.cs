using CommunityToolkit.Mvvm.ComponentModel;
using FinantsApp.Data;
using FinantsApp.Models;
using FinantsApp.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinantsApp;

public partial class SavingGoalPage : ContentPage
{
	private readonly DatabaseContext _vm;

    public SavingGoal? CurrentGoal { get; set; }
    public ObservableCollection<SavingGoal> PreviousGoals { get; set; } = [];

	public SavingGoalPage(DatabaseContext vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        CurrentGoal = null;
        PreviousGoals = [];

        IEnumerable<SavingGoal> list = await _vm.GetAllAsync<SavingGoal>();
        foreach (var item in list)
        {
            if (item.State == GoalState.Active)
            {
                CurrentGoal = item;
            }

            PreviousGoals.Add(item);
        }

        UpdateCurrentGoalDisplay();
    }


    private void UpdateCurrentGoalDisplay()
    {
        if (CurrentGoal == null)
        {
            CurrentGoalSection.IsVisible = false;
        }
        else
        {
        }


    }

    private async void OnNewGoalClicked(object sender, EventArgs e)
    {

    }

    private async void OnRemoveGoalClicked(object sender, EventArgs e)
    {

    }
}