using Godot;
using System;

public partial class EnemyFollowState : EnemyState
{
    [Export]
    public float minMoveSpeed = 20.0f;
    [Export]
    public float maxMoveSpeed = 30.0f;
    [Export]
    public float stopDistance = 10.0f;

    public float speed;

    public override void EnterState()
    {
        speed = (float)GD.RandRange(minMoveSpeed, maxMoveSpeed);
    }

    public override void UpdateState(double delta)
    {
        if(enemy == null || Refs.Instance.player == null)
        {
            return;
        }

        Vector2 direction = enemy.GlobalPosition.DirectionTo(Refs.Instance.player.GlobalPosition);
        float distance = enemy.GlobalPosition.DistanceTo(Refs.Instance.player.GlobalPosition);

        if(distance > stopDistance)
        {
            enemy.GlobalPosition += direction * speed * (float)delta;
            enemy.UpdataAnimation(direction);
        }

        if(distance <= stopDistance)
        {
            fsm.ChangeState("Attack");
        }
    }
}
