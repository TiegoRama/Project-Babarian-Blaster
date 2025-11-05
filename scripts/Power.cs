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

	private Turret turret;

	private Enemy enemy;

	private MeshInstance3D damageUppp;

	public override void _Ready()
	{
		enemy = GetTree().GetFirstNodeInGroup("Enemy") as Enemy;
		turret = GetTree().GetFirstNodeInGroup("Turret") as Turret;
		damageUppp = GetNode<MeshInstance3D>("DamageUPPP");
	}

	public void RandomLoot(Vector3 dropPosition)
	{
		var chance = GD.RandRange(0, 100);
		if (chance < 95)
		{
			damageUppp.GlobalPosition = dropPosition;
			
			Tween tween = CreateTween();
			tween.TweenProperty(damageUppp, "visible", true, 0.5).SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
			tween.TweenProperty(damageUppp, "visible", false, 0.5).SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
		}
	}
}