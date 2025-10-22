using Godot;
using System;
using System.IO;
using System.Linq;

public partial class Turret : Node3D
{
	[Export] private PackedScene projectileScene;
	public Path3D EnemyPath;
	private Node3D cannonShot;
	private PathFollow3D Target;
	[Export(PropertyHint.Range, "5,100,1")] private float TurretRange = 10.0f;
	private AnimationPlayer animationPlayer;
	private Label3D upArrow;
	private float damageMultiplier = 1.0f;
	private Node3D turretBase;

	public override void _Ready()
	{
		cannonShot = GetNode<Node3D>("TurretBase/TurretTop/Cannon/CannonShot");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		turretBase = GetNode<Node3D>("TurretBase");
		AddToGroup("Turret");
		upArrow = GetNode<Label3D>("UpArrow");
	}

	public override void _PhysicsProcess(double delta)
	{
		Target = find_best_target();
		if (Target != null)
		{

			turretBase.LookAt(Target.GlobalPosition, Vector3.Up, true);
		}
	}
	private void _on_timer_timeout()
	{
		if (Target != null)
		{

			var shot = projectileScene.Instantiate<Projectile>();
			AddChild(shot);

			shot.GlobalTransform = cannonShot.GlobalTransform;
			shot.direction = turretBase.GlobalTransform.Basis.Z;
			shot.Damage *= damageMultiplier;
			animationPlayer.Play("Fire");
			GD.Print(shot.Damage);

		}


	}
	private PathFollow3D find_best_target()
	{
		PathFollow3D bestTarget = null;
		float BestProgress = 0.0f;

		foreach (var enemy in EnemyPath.GetChildren())
		{
			if (enemy is PathFollow3D enemyPath)
			{

				if (enemyPath.Progress > BestProgress && GlobalPosition.DistanceTo(enemyPath.GlobalPosition) <= TurretRange)
				{
					bestTarget = enemyPath;
					BestProgress = enemyPath.Progress;

				}
			}
		}

		return bestTarget;
	}

	public void UpgradeTurretDamage()
	{
		damageMultiplier += 0.1f;

		upArrow.Visible = true;
		upArrow.Scale = Vector3.Zero;

		Tween tween = CreateTween();

		tween.TweenProperty(upArrow, "scale", Vector3.One, 0.3f)
			 .SetEase(Tween.EaseType.Out)
			 .SetTrans(Tween.TransitionType.Back);

		tween.TweenInterval(0.5f);

		tween.TweenProperty(upArrow, "scale", Vector3.Zero, 0.2f);

		tween.TweenCallback(Callable.From(() => upArrow.Visible = false));
	}
}
