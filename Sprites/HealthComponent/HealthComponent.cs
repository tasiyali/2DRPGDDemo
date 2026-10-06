using Godot;
using System;

public partial class HealthComponent : Node
{
    //声明一个信号，当生命值发生变化时触发
    [Signal]
    public delegate void OnHealthChangedEventHandler(float currentHealth);
    //声明一个信号，当生命值为0时触发
    [Signal]
    public delegate void OnDeathEventHandler();

    public float currentHealth;
    public float maxHealth;

    //设置最大生命值和当前生命值
    public void Setup(float Value)
    {
        maxHealth = Value;
        currentHealth = Value;
    }

    //减少生命值并检查是否死亡
    public void TakeDamage(float damage)
    {
        if(currentHealth <= 0)
            return;
        
        //减少当前生命值，确保不会低于0
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        //发射生命值变化信号
        EmitSignal(SignalName.OnHealthChanged, currentHealth);
        if(currentHealth <= 0)
        {
            //发射死亡信号
            EmitSignal(SignalName.OnDeath);
        }
    }

    public void Heal(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        EmitSignal(SignalName.OnHealthChanged, currentHealth);
    }
}
