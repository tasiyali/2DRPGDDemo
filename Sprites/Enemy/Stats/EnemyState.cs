using Godot;
using System;

public partial class EnemyState : State
{
    public Enemy enemy;

    public override async void _Ready()
    {
        base._Ready();
        //等待所有者就绪
        await ToSignal(Owner, Node.SignalName.Ready);
        //获取Player实例
        enemy = Owner as Enemy;
    }
}
