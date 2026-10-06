using Godot;
using System;

public partial class PlayerState : State
{
    public Player player;

    public override async void _Ready()
    {
        //等待所有者就绪
        await ToSignal(Owner, Node.SignalName.Ready);
        //获取Player实例
        player = Owner as Player;
    } 
}
