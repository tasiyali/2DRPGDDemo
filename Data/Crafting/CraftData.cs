using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class CraftData : Resource
{
    [Export]
    public ItemData craftItem;
    [Export]
    public Array<CraftMaterial> craftMaterial;
}
