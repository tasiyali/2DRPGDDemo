using Godot;
using System;

public partial class Refs : Node
{
    public TileMapLayer navigation;
    public Player player;
    public HUD hUD;

    private static PackedScene damegeFX = GD.Load<PackedScene>("res://Scenes/Effects/damage_fx.tscn");
    private static PackedScene damgeTEXT = GD.Load<PackedScene>("res://Scenes/Effects/damage_text.tscn");
    private static PackedScene newLevelFX = GD.Load<PackedScene>("res://Scenes/Effects/new_level_fx.tscn");
    public static PackedScene dropItem = GD.Load<PackedScene>("res://Scenes/DropItem/drop_item.tscn");
    public static PackedScene shopBntton = GD.Load<PackedScene>("res://Scenes/UI/ShopPanel/shop_button.tscn");
    public static PackedScene craftButton = GD.Load<PackedScene>("res://Scenes/UI/CraftPanel/craft_button.tscn");
    public static PackedScene questButton = GD.Load<PackedScene>("res://Scenes/UI/QuestPanel/quest_button.tscn");

    public void CreateDamageFx(Vector2 pos)
    {
        CreateAtPos(damegeFX, pos);
    }

    public void CreateNewLevelFx(Vector2 pos)
    {
        CreateAtPos(newLevelFX, pos);
    }

    public void CreateDamageText(Vector2 pos, float value)
    {
        Label label = damgeTEXT.Instantiate<Label>();
        label.Text = ((int)value).ToString();
        label.GlobalPosition = pos + Vector2.Right.Rotated((float)GD.RandRange(0, MathF.Tau)) * 2;
        GetTree().Root.AddChild(label);

        Tween tween = CreateTween();
        tween.TweenProperty(label, "global_position:y", label.GlobalPosition.Y - 24, 0.7);
        tween.Finished += () => label.QueueFree();
    }

    public void CreateAtPos(PackedScene scene, Vector2 pos)
    {
        AnimatedSprite2D fxScene = scene.Instantiate<AnimatedSprite2D>();
        fxScene.GlobalPosition = pos;
        GetTree().Root.AddChild(fxScene);
        fxScene.AnimationFinished += () => fxScene.QueueFree();
    }

    // ✅ 单例模式
    private static Refs _instance;
    public static Refs Instance => _instance;
    
    public override void _EnterTree()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            QueueFree();
        }
    }
}
