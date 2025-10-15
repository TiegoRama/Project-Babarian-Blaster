using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Enemy : PathFollow3D
{
    [Export(PropertyHint.Range, "0,50,1")] public float speed = 5.0f;
    [Export(PropertyHint.Range, "5,50,1")] private float max_health;
    private float _currentHealth;
    public float currentHealth{
        get {
            
           
            return _currentHealth; 
            }
        set
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                QueueFree();
            }
        } 
    }

    private Node3D baseNode;
    public override void _Ready()
    {
        currentHealth = max_health;
        baseNode = GetTree().GetFirstNodeInGroup("Base") as Node3D;
    }
    public override void _Process(double delta)
    {

        Progress += (float)delta * speed;
        if (ProgressRatio >= 1.0f)
        {
            baseNode.Call("TakeDamage", 1);
            QueueFree();
        }
    }
    
}

