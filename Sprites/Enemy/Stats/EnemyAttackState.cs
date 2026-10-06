using Godot;
using System;

public partial class EnemyAttackState : EnemyState
{
    [Export]
    public float attackDuration = 0.5f;

    public float attackTimer = 0.0f;
    public bool isAttack = false;

    public override void EnterState()
    {
        attackTimer = attackDuration;
        isAttack = false;
    }

    public override void UpdateState(double delta)
    {
        if(enemy == null || !IsInstanceValid(Refs.Instance.player))
        {
            fsm.ChangeState("Wander");
            return;
        }

        attackTimer -= (float)delta;

        if(attackTimer <= attackDuration / 2 && !isAttack)
        {
            ApplyDamage();
            isAttack = true;
        }

        if(attackTimer <= 0)
        {
            fsm.ChangeState("Follow");
        }
    }

    public void ApplyDamage()
    {
        float distance = enemy.GlobalPosition.DistanceTo(Refs.Instance.player.GlobalPosition);
        if(distance <= 25)
        {
            Refs.Instance.CreateDamageFx(Refs.Instance.player.GlobalPosition);
            Refs.Instance.CreateDamageText(Refs.Instance.player.GlobalPosition, enemy.damage);
            Refs.Instance.player.HealthComponent.TakeDamage(enemy.damage);
            SoundManager.Instance.Play((int)E_Sound.Import);
        }
    }
}
