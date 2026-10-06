using Godot;
using System;

public partial class CraftButton : Button
{
    [Export]
    public TextureRect ItemIcon { get; set;}

    public CraftData data;

    public void LoadData(CraftData craftData)
    {
        data = craftData;
        ItemIcon.Texture = data.craftItem.itemIron;
    }
}
