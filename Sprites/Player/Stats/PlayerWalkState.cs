using Godot;
using System;

public partial class PlayerWalkState : PlayerState
{
    //在进入状态时播放walk动画
    public override void EnterState()
    {
        player.PlayDirectionAnimation("walk");
    }

    //在更新状态时检查输入以决定是否切换到其他状态
    public override void UpdateState(double delta)
    {
        Vector2 inputVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        //如果按下攻击键，切换到攻击状态
        if (Input.IsActionPressed("attack"))
        {
            fsm.ChangeState("Attack");
            return;
        }

        //如果没有输入方向，切换到Idle状态
        if(inputVector == Vector2.Zero)
        {
            fsm.ChangeState("Idle");
            return;
        }
        //更新玩家的移动方向并播放walk动画
        player.UpdateDirection(inputVector);
        player.PlayDirectionAnimation("walk");

        //移动玩家
        player.Velocity = inputVector * player.moveSpeed;
        player.MoveAndSlide();
    }
}
