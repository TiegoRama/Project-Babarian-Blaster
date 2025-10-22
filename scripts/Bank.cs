using Godot;
using System;

public partial class Bank : MarginContainer
{
    [Export] private int startingGold = 300;
    private int gold;
    private Label goldLabel;
    public int Gold
    {
        get
        {

            return gold;
        }
        set
        {
            if (gold < 0)
            {
                gold = 0;
            }
            gold = value;
            goldLabel = GetNode<Label>("Label");
            goldLabel.Text = $"Gold : {gold}";
        }
    }
    public override void _Ready()
    {
        Gold = startingGold;
    }
}
