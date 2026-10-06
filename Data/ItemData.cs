using Godot;
using System;

[GlobalClass]
public partial class ItemData : Item
{
    [Export]
    public Texture2D itemIron;
    [Export]
    public string itemName;
    [Export(PropertyHint.MultilineText)]
    public string itemDescription;
    [Export]
    public int itemPrice;

    [ExportGroup("Consumable")]
    [Export]
    public float value;
}
