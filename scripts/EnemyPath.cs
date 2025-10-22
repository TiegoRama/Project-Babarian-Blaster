using Godot;
using System;

public partial class EnemyPath : Path3D
{
	[Export] private PackedScene enemyScene;
	[Export] private DifficultyManager difficultyManager;

	[Export] private CanvasLayer victoryScreen;
	private Timer timer;





	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
	}
	private void SpawnEnemy()
	{
		var enemy = enemyScene.Instantiate<Enemy>();
		enemy.maxHealth = difficultyManager.GetEnemyHealthMultiplier();
		AddChild(enemy);
		timer.WaitTime = difficultyManager.GetSpawnTime();
		enemy.TreeExited += () => EnemyDefeated();
	}
	private void on_difficulty_manager_stop_spawning_enemies()
	{
		timer.Stop();
	}

	private void EnemyDefeated()
    {
		if (timer.IsStopped())
		{
			for (int i = 0; i < GetChildren().Count; i++)
			{
				var child = GetChild(i);
				if (child is Enemy)
				{
					return;
				}
			}
			victoryScreen.Call("Victory");
		}
		
    }
}
