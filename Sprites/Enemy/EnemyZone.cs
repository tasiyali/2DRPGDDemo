using Godot;
using System;

public partial class EnemyZone : Area2D
{
    [Export]
    public CollisionShape2D Collider { get; set;}
    [Export]
    public PackedScene EnemyScene { get; set;}
    [Export]
    public float spawnRate = 3.0f;
    [Export]
    public int maxEnemies = 3;

    public int currentEnemies = 0;

    public override void _Ready()
    {
        base._Ready();
        Timer timer = new Timer();
        timer.WaitTime = spawnRate;
        timer.Autostart = true;
        timer.Timeout += OnTimeTimeout;
        AddChild(timer);
    }

    public void spawnEnemy()
    {
        if(currentEnemies >= maxEnemies)
        {
            return;
        }

        Vector2 pos = GetRandomPoint();
        Enemy enemy = EnemyScene.Instantiate<Enemy>();
        enemy.GlobalPosition = pos;
        enemy.EnemyZone = this;
        //EventBus.Instance.OnEnemyDied += OnEnemyDied;
        GetTree().Root.AddChild(enemy);
        currentEnemies += 1;
    }

    public Vector2 GetRandomPoint()
    {
        RectangleShape2D shape = Collider.Shape as RectangleShape2D;
        Vector2 halfSize = shape.Size / 2.0f;
        Vector2 randomPos = new Vector2(
            (float)GD.RandRange(-halfSize.X, halfSize.X),
            (float)GD.RandRange(-halfSize.Y, halfSize.Y)
        );
        return Collider.GlobalPosition + randomPos;
    }

    public void OnEnemyDied()
    {
        currentEnemies -= 1;
        if(currentEnemies < 0)
        {
            currentEnemies = 0;
        }
    }

    public void OnTimeTimeout()
    {
        spawnEnemy();
    }

}
