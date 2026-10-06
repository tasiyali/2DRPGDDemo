using Godot;
using System;

public partial class SkillEffect : Node2D
{

    [Export]
    public AnimatedSprite2D sprite2D { set; get; }

    public override void _Ready()
    {
        sprite2D.Frame = 0;
        sprite2D.Play("default");
    }

    public void OnAnimationFinished()
    {
        QueueFree();
    }
}
