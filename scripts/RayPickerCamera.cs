using Godot;
using System;

public partial class RayPickerCamera : Camera3D
{
    private Vector2 mouse_position;
    private RayCast3D ray_cast;
    [Export] private GridMap gridmap;
    [Export] private Node3D TurretManager;
    private Projectile Projectiles;
    public override void _Ready()

    {
        ray_cast = GetNode<RayCast3D>("RayCast3D");
       

        
    }

    public override void _Process(double delta)
    {
        mouse_position = GetViewport().GetMousePosition();
        Projectiles = GetTree().GetFirstNodeInGroup("Projectile") as Projectile;
        ray_cast.TargetPosition = ProjectLocalRayNormal(mouse_position) * 100;
        if (ray_cast.GetCollider() == gridmap)
        {

            Vector3 collision_point = ray_cast.GetCollisionPoint();
            Vector3I cell = gridmap.LocalToMap(collision_point);
            if (gridmap.GetCellItem(cell) == 0)
            {
                Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
                if (Input.IsActionJustPressed("left_click"))
                {
                    gridmap.SetCellItem(cell, 1);
                    Vector3 TilePosition = gridmap.MapToLocal(cell);
                    TurretManager.Call("BuildTurret", TilePosition);
                }
            }
        }
       
        else
        {
            Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
        }

    }

}
