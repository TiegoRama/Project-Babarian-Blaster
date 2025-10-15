using Godot;
using System;
using System.Linq;

public partial class Turret : Node3D
{
	[Export] private PackedScene projectileScene;
	public Path3D EnemyPath;
	private MeshInstance3D Barrel;


	public override void _Ready()
	{
		Barrel = GetNode<MeshInstance3D>("TurretBase/TurretTop/Scope/Barrel");
	}

	public override void _PhysicsProcess(double delta)
	{
		var enemy = EnemyPath.GetChildren().Last() as Node3D;
		LookAt(enemy.GlobalPosition, Vector3.Up, true);
	}
	private void _on_timer_timeout()
	{
		var shot = projectileScene.Instantiate<Projectile>();
		AddChild(shot);
		shot.GlobalTransform = Barrel.GlobalTransform;
		shot.direction = GlobalTransform.Basis.Z;
		

	}
}
