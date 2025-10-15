using Godot;
using System;
using System.IO;
using System.Linq;

public partial class Turret : Node3D
{
	[Export] private PackedScene projectileScene;
	public Path3D EnemyPath;
	private MeshInstance3D Barrel;
	private PathFollow3D Target;
	[Export( PropertyHint.Range,"5,100,1")] private float TurretRange = 10.0f;

	public override void _Ready()
	{
		Barrel = GetNode<MeshInstance3D>("TurretBase/TurretTop/Scope/Barrel");
	}

	public override void _PhysicsProcess(double delta)
	{
		Target = find_best_target();
		if (Target != null)
		{
			LookAt(Target.GlobalPosition, Vector3.Up, true);
		}		
	}
	private void _on_timer_timeout()
	{
		if (Target != null)
		{
			
			var shot = projectileScene.Instantiate<Projectile>();
			AddChild(shot);
			shot.GlobalTransform = Barrel.GlobalTransform;
			shot.direction = GlobalTransform.Basis.Z;
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
}
