using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Enemy : PathFollow3D
{
    [Export] public float speed = 5.0f;

    private Node3D baseNode;
    public override void _Ready()
    {
        baseNode = GetTree().GetFirstNodeInGroup("Base") as Node3D;
    }
    public override void _Process(double delta)
    {

        Progress += (float)delta * speed;
        if (ProgressRatio >= 1.0f){
            baseNode.Call("TakeDamage", 1);
            Progress = 0.0f;
        }
    }
}