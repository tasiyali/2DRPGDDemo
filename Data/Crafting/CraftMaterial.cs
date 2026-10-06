using Godot;
using System;

[GlobalClass]
public partial class CraftMaterial : Resource
{
    [Export]
    public ItemData item;
    [Export]
    public int amount;
}
