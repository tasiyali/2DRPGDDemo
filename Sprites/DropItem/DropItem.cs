using Godot;
using System;
using System.Collections;

public partial class DropItem : Area2D
{
    [Export]
    public ItemData item;
    [Export]
    public int amount;

    [Export]
    public float shineSpeed = 0.6f;
    public float shineFreq = 2.0f;

    [Export]
    public Sprite2D sprite { get; set;}

    public override void _Ready()
    {
        base._Ready();
        ShineItem();
        SetUp();
    }

    public void SetUp()
    {
        if(item != null && item.itemIron != null)
        {
            sprite.Texture = item.itemIron;
        }
    }

    public void LoadData(LootData data)
    {
        item = data.item;
        amount = data.amount;
    }

    public void ShineItem()
    {
        Tween tween = CreateTween().SetLoops();
        tween.TweenProperty(sprite.Material, "shader_parameter/shine_progress", 1.0, shineSpeed).SetDelay(shineFreq);
        tween.TweenProperty(sprite.Material, "shader_parameter/shine_progress", 0.0, 0.0);
    }

    public void OnBodyEntered(Node2D body)
    {
        Inventory.Instance.AddItem(item, amount);
        QueueFree();
    }    
}
