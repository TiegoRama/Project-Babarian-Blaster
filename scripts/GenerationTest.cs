using Godot;
using System;
using System.Collections.Generic;

public partial class GenerationTest : Node3D
{
	[Export] private int width;
	[Export] private int depth;
	private const int TURRET = 1;
	private const int TREE = 2;
	private const int ROCK = 3;
	private const int BASE = 4;
	private const int FREESPACE = 0;

	private Vector3I basePosition;
	private Vector3 basePositionWorld;
    private Random random;

	private List<Vector3I> pathCells = new List<Vector3I>();



	[Export] private GridMap gridMap;
	[Export] private Path3D enemyPath;


	public override void _Ready()
	{
		Random random = new Random();
		basePosition = new Vector3I(random.Next(-8, 8), 0, random.Next(-8, 8));
		basePositionWorld = gridMap.MapToLocal(basePosition);
		basePositionWorld.Y = 0.5f;
		CreateBaseTerrain();
		CreateNaturalTerrain();
		CreatePathEnemy();
		GetPathCells();
	}





	public void CreateBaseTerrain()
	{
		for (int x = width; x < 8; x++)
		{
			for (int z = depth; z < 8; z++)
			{

				Vector3I pos = new Vector3I(x, 0, z);
				var chance = GD.RandRange(0, 100);
				if (chance < 10)
				{
					gridMap.SetCellItem(pos, TREE);
				}
				else if (chance >= 10 && chance < 20)
				{
					gridMap.SetCellItem(pos, ROCK);
				}
				else
				{
					gridMap.SetCellItem(pos, FREESPACE);
				}

				gridMap.SetCellItem(basePosition, BASE);
			}
		}	
	}



	public void CreateNaturalTerrain()
	{
		FastNoiseLite noise = new FastNoiseLite();
		noise.Seed = 12345;

		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < depth; z++)
			{
				Vector3I pos = new Vector3I(x, 0, z);

				float noiseValue = noise.GetNoise2D(x, z);

				int cellType;

				if (noiseValue < -0.3f)
					cellType = TREE;  
				else if (noiseValue < 0.3f)
					cellType = ROCK;  
				else
					cellType = FREESPACE;  

				gridMap.SetCellItem(pos, cellType);
			}
		}
	}


	public void CreatePathEnemy()
	{
		var curve = new Curve3D();
		Random random = new Random();

		Vector3 startPoint = new Vector3(random.Next(-32, 32), 0.5f, random.Next(-32, 32));
		curve.AddPoint(startPoint);
		while (startPoint.X < 30)
		{
			startPoint.X += 3;


			if (random.NextDouble() < 0.3)
			{
				startPoint.Z += random.Next(-2, 3);
			}

			startPoint.Z = Mathf.Clamp(startPoint.Z, 2, 18);

			curve.AddPoint(startPoint);
		}
		curve.AddPoint(basePositionWorld);
		enemyPath.Curve = curve;

	}

	private void GetPathCells()
	{
		pathCells.Clear();
		Curve3D curve = enemyPath.Curve;
		float pathlength = curve.GetBakedLength();
		int samples = Mathf.CeilToInt(pathlength / 0.5f);

		for (int i = 0; i <= samples; i++)
		{
			float offset = (i / (float)samples) * pathlength;
			Vector3 point = curve.SampleBaked(offset);
			Vector3I cell = gridMap.LocalToMap(point);
			if (!pathCells.Contains(cell))
			{
				pathCells.Add(cell);
			}
		}
	}
	private bool IsNearPath(Vector3I cell, int distance)
	{
		foreach (var pathCell in pathCells)
		{
			int distSquared = cell.DistanceSquaredTo(pathCell);
			if (distSquared <= distance * distance)
			{
				return true;
			}
		}
		return false;
	}
		
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.Pressed && keyEvent.Keycode == Key.R)
			{
				GD.Print("🔄 Regenerating...");
				Regenerate();
			}
		}
	}
	void Regenerate()
	{
		GetTree().ReloadCurrentScene();

	}
	
}