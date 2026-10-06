using Godot;
using System;

public partial class EnemyWanderState : EnemyState
{
    [Export]
    public float minMoveSpeed = 20.0f;
    [Export]
    public float maxMoveSpeed = 30.0f;
    [Export]
    public float arrivalDistance = 5.0f;

    public Vector2 targetPos;

    public override void EnterState()
    {
        PickNewTarget();
    }

    public override void UpdateState(double delta)
    {
        base._Process(delta);
        if(enemy == null || enemy.EnemyZone == null)
        {
            return;
        }

        //朝目标行动
        Vector2 direction = enemy.GlobalPosition.DirectionTo(targetPos);
        float speed = (float)GD.RandRange(minMoveSpeed, maxMoveSpeed);
        enemy.GlobalPosition += direction * speed * (float)delta;

        //更新动画
        enemy.UpdataAnimation(direction);

        //检查是否到达位置
        if(enemy.GlobalPosition.DistanceTo(targetPos) < arrivalDistance)
        {
            PickNewTarget();
        }
    }

    public void PickNewTarget()
    {
        if(enemy != null && enemy.EnemyZone != null)
        {
            targetPos = enemy.EnemyZone.GetRandomPoint();
        }
    }
}
