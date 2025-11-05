using Godot;
using System;
using System.Text.RegularExpressions;

public partial class RayPickerCamera : Camera3D
{
    private Vector2 mouse_position;
    private RayCast3D ray_cast;
    [Export] private GridMap gridmap;
    [Export] private Node3D TurretManager;
     [Export(PropertyHint.Range, "1, 500, 1")]private float turretCost = 100.0f;

    private Projectile projectile;

    [Export(PropertyHint.Range, "1, 100, 1")] private float UpgradeTurretCost = 5.0f;
    private Bank bank;
    public override void _Ready()

    {
        ray_cast = GetNode<RayCast3D>("RayCast3D");
        bank = GetTree().GetFirstNodeInGroup("Bank") as Bank;
        bank.Gold = 500;
        projectile = GetTree().GetFirstNodeInGroup("Projectile") as Projectile;

    }

    public override void _Process(double delta)
    {
        mouse_position = GetViewport().GetMousePosition();
        ray_cast.TargetPosition = ProjectLocalRayNormal(mouse_position) * 100;
        if (ray_cast.GetCollider() == gridmap)
        {

            Vector3 collision_point = ray_cast.GetCollisionPoint();
            Vector3I cell = gridmap.LocalToMap(collision_point);
            if (gridmap.GetCellItem(cell) == 0)
            {
                if (bank.Gold >= turretCost)
                {

                    Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
                    if (Input.IsActionJustPressed("left_click"))
                    {
                        gridmap.SetCellItem(cell, 1);
                        Vector3 tilePosition = gridmap.MapToLocal(cell);
                        TurretManager.Call("BuildTurret", tilePosition);
                        bank.Gold -= (int)turretCost;
                    }
                }

            }
            else if (gridmap.GetCellItem(cell) == 1)
            {
                if (bank.Gold >= UpgradeTurretCost)
                {
                    Input.SetDefaultCursorShape(Input.CursorShape.Drag);
                    if (Input.IsActionJustPressed("right_click"))
                    {
                        Vector3 tilePosition = gridmap.MapToLocal(cell);
                        Turret turretToUpgrade = FindTurretAtPosition(tilePosition);
                        bank.Gold -= (int)UpgradeTurretCost;
                        turretToUpgrade.UpgradeTurretDamage();


                    }


                }
            }

            else
            {
                Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
            }

        }

    }
    
    private Turret FindTurretAtPosition(Vector3 position)
    {
        foreach (var turret in GetTree().GetNodesInGroup("Turret"))
        {
            if (turret is Turret t)
            {
                if (t.GlobalPosition.DistanceTo(position) < 0.1f)
                {
                    return t;
                }
            }
        }
        return null;
    }
}