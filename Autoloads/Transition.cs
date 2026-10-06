using Godot;
using System;
using System.Threading.Tasks;

public partial class Transition : Node2D
{
    [Export]
    public ColorRect ColorRect { get; set;}

    public Tween tween;

    public async Task FadeIn(float duration = 0.5f)
    {
        if(tween != null)
        {
            tween.Kill();
        }

        ((ShaderMaterial)ColorRect.Material).SetShaderParameter("progress", 0.0);
        tween = CreateTween();
        tween.TweenProperty(ColorRect.Material, "shader_parameter/progress", 1.0, duration);
        await ToSignal(tween, Tween.SignalName.Finished);
    }

    public async Task FadeOut(float duration = 0.5f)
    {
        if(tween != null)
        {
            tween.Kill();
        }

        ((ShaderMaterial)ColorRect.Material).SetShaderParameter("progress", 1.0);
        tween = CreateTween();
        tween.TweenProperty(ColorRect.Material, "shader_parameter/progress", 0.0, duration);
        await ToSignal(tween, Tween.SignalName.Finished);
    }
}
