using Godot;
using Godot.Collections;
using System;

public partial class PlayerAttackState : PlayerState
{
    //存储玩家武器在不同方向的旋转角度
    public Dictionary<string, float> weaponRotations = new Dictionary<string, float>
    {
        {"down", 180},
        {"up", 0},
        {"left", -90},
        {"right", 90}
    };

    //进入攻击状态时播放攻击动画并设置武器位置
    public override void EnterState()
    {
        player.PlayDirectionAnimation("attack");
        PositionWeapon();
        player.PlayerSprite.AnimationFinished += OnAnimationFinished;
    }

    public override void ExitState()
    {
        player.PlayerSprite.AnimationFinished -= OnAnimationFinished;
    }

    //根据玩家的当前方向设置武器的位置和旋转角度
    public void PositionWeapon()
    {
        string directionKey = player.lastDirection;
        Marker2D marker = (Marker2D)player.attackPositions[directionKey];
        player.Weapon.GlobalPosition = marker.GlobalPosition;
        player.Weapon.RotationDegrees = weaponRotations[directionKey];
        player.Weapon.Show();
        player.EnableWeaponCollision(true);
    }

    //当攻击动画播放完成时隐藏武器并切换回空闲状态
    public void OnAnimationFinished()
    {
        player.Weapon.Hide();
        player.EnableWeaponCollision(false);
        fsm.ChangeState("Idle");
    }
}
