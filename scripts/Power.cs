using Godot;
using System;

public partial class Power : Node
{
	private float damageMultiplier;
	private float healhMultipler;
	private float speedMultiplier;
	private float fireRateMultiplier;
	private float goldMultiplier;
	private float experienceMultiplier;

	private Enemy enemy;

	public override void _Ready()
	{
		enemy = GetTree().GetFirstNodeInGroup("Enemy") as Enemy;
	}

	public void IncreaseDamage()
	{
		
		GD.Print("Damage Multiplier increased to: ");
	}
	

}
