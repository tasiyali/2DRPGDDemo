using Godot;
using System;

[GlobalClass]
public partial class LootData : Resource
{
    [Export]
    public ItemData item;
    [Export]
    public int amount;
}
