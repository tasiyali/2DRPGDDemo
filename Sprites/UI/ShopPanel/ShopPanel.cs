using Godot;
using Godot.Collections;
using System;

public partial class ShopPanel : Control
{
    [Export]
    public Array<ItemData> items;

    [Export]
    public GridContainer container { get; set;}
    [Export]
    public Label Totalcoin { get; set;}

    public override void _Ready()
    {
        base._Ready();
        foreach(Node child in container.GetChildren())
        {
            child.QueueFree();
        }
        LoadShopItems();
        Totalcoin.Text = GameData.Instance.coins.ToString();
    }

    public void LoadShopItems()
    {
        foreach(ItemData item in items)
        {
            ShopButton shopButton = Refs.shopBntton.Instantiate<ShopButton>();
            container.AddChild(shopButton);
            shopButton.LoadItem(item);
            shopButton.OnItemPurchased += OnItemPurchased;
        }
    }

    public void OnItemPurchased()
    {
        Totalcoin.Text = GameData.Instance.coins.ToString();
    }

    public void OnPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        Hide();
    }
}
