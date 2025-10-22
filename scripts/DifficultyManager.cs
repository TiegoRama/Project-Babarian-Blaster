using Godot;
using System;

public partial class DifficultyManager : Node
{
	[Export] private float gameLenght = 30.0f;
	[Export] private Curve spawnTimeCurve;

	[Export] private Curve enemyHealthCurve;
	private Timer timer;
	[Signal] delegate void stopSpawningEnemiesEventHandler();

	public override void _Ready()
    {
		timer = GetNode<Timer>("Timer");
		timer.Start(gameLenght);
    }

	private float GameProgressRatio()
	{
		return 1.0f - (float)timer.TimeLeft / gameLenght;
	}

	public float GetSpawnTime()
	{
		return spawnTimeCurve.Sample(GameProgressRatio());
	}

	public float GetEnemyHealthMultiplier()
	{
		return enemyHealthCurve.Sample(GameProgressRatio());
	}
	public void _on_timer_timeout()
	{
		EmitSignal(SignalName.stopSpawningEnemies);
	}	
}
