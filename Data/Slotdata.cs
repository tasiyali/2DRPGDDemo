using Godot;
using System;

public partial class Slotdata : Resource
{
    [Export]
    public ItemData item;
    [Export]
    public int quantity = 1;

    public Slotdata()
    {
        
    }

    public Slotdata(ItemData _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
    }
}
