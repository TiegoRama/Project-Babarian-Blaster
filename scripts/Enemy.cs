using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Enemy : PathFollow3D
{
    [Export(PropertyHint.Range, "0,50,1")] public float speed = 5.0f;
    [Export(PropertyHint.Range, "0,50,1")] private float health = 3.0f;

    private Node3D baseNode;
    public override void _Ready()
    {
        baseNode = GetTree().GetFirstNodeInGroup("Base") as Node3D;
    }
    public override void _Process(double delta)
    {

        Progress += (float)delta * speed;
        if (ProgressRatio >= 1.0f)
        {
            baseNode.Call("TakeDamage", 1);
            Progress = 0.0f;
        }
    }
    private void _on_area_3d_area_entered(Area3D area)
    {
        if (area is Projectile)
        {
            health -= 1;
            if (health <= 0)
            {
                QueueFree();
            }
        }
    }
}