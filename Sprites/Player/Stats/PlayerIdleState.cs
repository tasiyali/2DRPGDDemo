using Godot;
using System;

public partial class PlayerIdleState : PlayerState
{
    //在进入状态时播放idle动画
    public override void EnterState()
    {
        player.PlayDirectionAnimation("idle");
    }

    //处理玩家的输入事件，当按下“attack”键时切换到攻击状态
    public void _input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            fsm.ChangeState("Attack");
            return;
        }
    }

    //在每一帧中调用当前状态的UpdateState方法
    public override void UpdateState(double delta)
    {
        //检查输入以决定是否切换到其他状态
        if(player.IsMoving())
        {
            fsm.ChangeState("Walk");
        }
    }
}
