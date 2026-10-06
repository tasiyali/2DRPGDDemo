using Godot;
using Godot.Collections;
using System;

public partial class EquipmentSlot : Button
{
    [Export]
    public E_EquipType equipType;
    [Export]
    public TextureRect ItemIron { get; set;}

    public EquipData equippedItem;

    public override void _Ready()
    {
        base._Ready();
        ClearSlot();
    }

    public void LoadData(EquipData item)
    {
        equippedItem = item;
        if(item != null)
        {
            ItemIron.Texture = item.itemIron;
            ItemIron.Show();
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        equippedItem = null;
        ItemIron.Hide();
    }
}
