using Godot;
using System;

public partial class Town : Node2D
{
    [Export]
    public PackedScene PlayerScene { get; set;}
    [Export]
    public TileMapLayer Navigation { get; set; }
    [Export]
    public Marker2D SpawnPos { get; set;}

    public override void _Ready()
    {
        base._Ready();
        EventBus.Instance.OnInventoryUsedItem += OnInventoryUsedItem;
        CreatePlayer();
        Refs.Instance.navigation = Navigation;
    }

    public void CreatePlayer()
    {
        Player player = PlayerScene.Instantiate<Player>();
        AddChild(player);
        player.Setup();
        Refs.Instance.player = player;
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerCreated);
    }

    public void OnInventoryUsedItem(ItemData item)
    {
        switch (item.itemId)
        {
            case "HpPotion":
                Refs.Instance.player.HealthComponent.Heal(item.value);
                break;
            case "ManaPotion":
                Refs.Instance.player.AddMana(item.value);
                break;
        }
    }
}
