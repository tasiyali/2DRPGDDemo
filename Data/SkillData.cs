using Godot;
using System;

[GlobalClass]
public partial class SkillData : ItemData
{
    [Export]
    public float manaCost;
    [Export]
    public float baseDamage;
    [Export]
    public PackedScene skillEffectScene;
}
