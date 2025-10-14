using Godot;
using System;

public partial class Base : Node3D
{
    [Export(PropertyHint.Range, "1,1000,1" )] int max_health { get; set; } = 5;
    private Label3D displayhp;
    private float pourcentagehp;
    private int _currentHealth;
    public int currentHealth
    {
        get
        {
            displayhp.Modulate = Colors.Red.Lerp(Colors.White, (float)_currentHealth / (float)max_health);
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            GD.Print("health was changed");
        }
    }
    
    public override void _Ready()
    {
        currentHealth = max_health;
        displayhp = GetNode<Label3D>("MaxHP");
        displayhp.Text = $"{currentHealth} /  {max_health}";


    }
    public void TakeDamage(int damage)
    {
       
        currentHealth -= damage;
        displayhp.Text = $"{currentHealth} /  {max_health}";
        if (currentHealth <= 0)
        {
            GetTree().ReloadCurrentScene();
        }
        
    }
}
