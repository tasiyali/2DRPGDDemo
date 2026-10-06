using Godot;
using System;

public partial class ShopButton : Button
{
    [Signal]
    public delegate void OnItemPurchasedEventHandler();

    [Export]
    public TextureRect ItemIcon { get; set;}
    [Export]
    public Label Price { get; set;}

    public ItemData item;

    public void LoadItem(ItemData data)
    {
        item = data;
        ItemIcon.Texture = data.itemIron;
        Price.Text = data.itemPrice.ToString();
    }

    public void OnPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        if(GameData.Instance.coins < item.itemPrice)
        {
            return;
        }

        GameData.Instance.coins -= item.itemPrice;
        Inventory.Instance.AddItem(item);
        EmitSignal(SignalName.OnItemPurchased);
    }
}
