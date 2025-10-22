using Godot;
using System;

public partial class VictoryLayer : CanvasLayer
{

	private TextureRect star1;
	private TextureRect star2;
	private TextureRect star3;
	private Base baseNode;
	private Label parfaitLabel;
	private Label richeLabel;
	private Bank bank;


	public override void _Ready()
	{
		star1 = GetNode<TextureRect>("%Star1");
		star2 = GetNode<TextureRect>("%Star2");
		star3 = GetNode<TextureRect>("%Star3");
		baseNode = GetTree().GetFirstNodeInGroup("Base") as Base;
		parfaitLabel = GetNode<Label>("%Parfait");
		richeLabel = GetNode<Label>("%Riche");
		bank = GetTree().GetFirstNodeInGroup("Bank") as Bank;

	}

	private void Victory()
	{
		Visible = true;
		if (baseNode.max_health == baseNode.currentHealth)
		{
			star2.Modulate = Colors.White;
			parfaitLabel.Visible = true;
		}
		if (bank.Gold >= 500)
		{
			if (star2.Modulate != Colors.White)
			{
				star2.Modulate = Colors.White;
				richeLabel.Visible = true;
			}
			else
			{
				star3.Modulate = Colors.White;
				richeLabel.Visible = true;
			}
			
			
		}
	}	
	private void _on_retry_pressed()
	{
		GetTree().ReloadCurrentScene();
	}
	
	private void _on_quit_pressed()
    {
		GetTree().Quit();
    }
}
