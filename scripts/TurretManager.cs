using Godot;
using System;

public partial class TurretManager : Node3D

{
    [Export] private PackedScene TurretScene;
    [Export] public Path3D EnemyPath; 

    public override void _Ready()
    {
        
    }
    
    public void BuildTurret(Vector3 TurretPosition)
    {
        var NewTurret = TurretScene.Instantiate<Turret>();
        AddChild(NewTurret);
        NewTurret.GlobalPosition = TurretPosition;
        NewTurret.EnemyPath = EnemyPath;
    }








}


