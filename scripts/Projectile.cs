using Godot;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

public partial class Projectile : Area3D
{
	public Vector3 direction = Vector3.Forward;
	[Export(PropertyHint.Range, "0,500,1")] private float speed = 10.0f;
	[Export(PropertyHint.Range, "1,100,1")] private float damage;

	public float Damage {
		get { return damage; }

		set
		{
			damage = value;
		}
	}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta;
	}


	private void _on_area_entered(Area3D area)
	{

		if (area.IsInGroup("Enemy"))
		{
			var enemy = area.GetParent() as Enemy;
			enemy.currentHealth -= Damage;
			

			QueueFree();

		}
	}

	private void on_timer_timeout()
	{
		QueueFree();
	}
}


