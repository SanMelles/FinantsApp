namespace FinantsApp.Models;

using SQLite;
using System;

public class SavingGoal
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }
    public GoalState State { get; set; } = GoalState.Active;
    public int Goal { get; set; }
    public DateTime CreationTime { get; set; }
    public int DurationInDays { get; set; }
}

public enum GoalState
{
    Active,
    Deactivated,
    Expired
}
