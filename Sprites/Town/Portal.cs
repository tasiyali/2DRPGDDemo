using Godot;
using System;
using System.Threading.Tasks;

public partial class Portal : Area2D
{
    [Export]
    public Node2D target;

    public async void OnBodyEntered(Node2D body)
    {
        Transition transition = GetNode<Transition>("/root/Transition");
        await transition.FadeIn(1.0f);
        Refs.Instance.player.GlobalPosition = target.GlobalPosition;
        await transition.FadeOut(1.0f);
    }
}
