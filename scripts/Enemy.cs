using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Enemy : PathFollow3D
{
    [Export(PropertyHint.Range, "0,50,1")] public float speed = 5.0f;
    [Export(PropertyHint.Range, "5,50,1")] public float maxHealth;
    [Export] private int dropGold = 10;
    private float damage = 1.0f;
    private Bank bank;
    private Power power;

    private AnimationPlayer animationPlayer;
    private float _currentHealth;
    public float currentHealth
    {
        get
        {


            return _currentHealth;
        }
        set
        {
            _currentHealth = value;

            if (_currentHealth < maxHealth)
            {
                animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
                animationPlayer.Play("TakeDamage");
            }

            if (_currentHealth <= 0)
            {
                bank = GetTree().GetFirstNodeInGroup("Bank") as Bank;
                bank.Gold += dropGold;
                QueueFree();
                Drop();
            }
        }
    }

    private Node3D baseNode;
    public override void _Ready()
    {
        currentHealth = maxHealth;
        baseNode = GetTree().GetFirstNodeInGroup("Base") as Node3D;
        power = GetTree().GetFirstNodeInGroup("Power") as Power;
    }
    public override void _Process(double delta)
    {
        Progress += (float)delta * speed;
        if (ProgressRatio >= 1.0f)
        {
            baseNode.Call("TakeDamage", damage);
            QueueFree();
        }
    }

    private void Drop()
    {
  
            Random rnd = new Random();
            int chance = rnd.Next(0, 100);
            if (chance < 99)

        {
            power.IncreaseDamage();
        }

    }
}
