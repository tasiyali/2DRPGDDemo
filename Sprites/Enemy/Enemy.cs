using Godot;
using Godot.Collections;
using System;

public partial class Enemy : Area2D
{
    [Export]
    public float maxHealth = 10.0f;
    [Export]
    public float damage = 2.0f;
    [Export]
    public float expAmount = 10.0f;
    [Export]
    public Array<LootData> lootDatas = new Array<LootData>();

    [Export]
    public HealthComponent HealthComponent { get; set;}
    [Export]
    public Sprite2D Selector { get; set;}
    [Export]
    public EnemyZone EnemyZone { get; set;}
    [Export]
    public Fsm FSM { get; set;}
    [Export]
    public AnimatedSprite2D AnimatedSprite { get; set;}
    [Export]
    public ProgressBar HealthBar { get; set;}

    public override void _Ready()
    {
        base._Ready();
        HealthComponent.Setup(maxHealth);
        HealthBar.Value = 1.0f;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(FSM.currentState != null)
        {
            FSM.currentState.UpdateState(delta);
        }
    }

    public void UpdataAnimation(Vector2 dir)
    {
        if(Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))
        {
            if(dir.X > 0)
            {
                AnimatedSprite.Play("walk_right");
            }
            else
            {
                AnimatedSprite.Play("walk_left");
            }
        }
        else
        {
            if(dir.Y > 0)
            {
                AnimatedSprite.Play("walk_down");
            }
            else
            {
                AnimatedSprite.Play("walk_up");
            }
        }
    }

    public void DropLoot()
    {
        LootData randomData = lootDatas.PickRandom();
        DropItem dropItem = Refs.dropItem.Instantiate<DropItem>();

        // 设置掉落物品的位置
        Vector2 awayDir = (GlobalPosition - Refs.Instance.player.GlobalPosition).Normalized();
        Vector2 dropPos = GlobalPosition + awayDir * 16.0f;

        dropItem.LoadData(randomData);
        dropItem.GlobalPosition = dropPos;
        GetTree().Root.CallDeferred(Node.MethodName.AddChild, dropItem);

        Refs.Instance.player.AddExp(expAmount);
        EnemyZone?.OnEnemyDied();
    }

    //显示选中框
    public void SelectorEnemy()
    {
        Selector.Show();
    }

    //隐藏选中框
    public void DeselectEnemy()
    {
        Selector.Hide();
    }

    public void OnBodyEntered(Node2D body)
    {
        if(body is Player)
        {
            FSM.ChangeState("Follow");
        }
    }

    public void OnBodyExited(Node2D body)
    {
        if(body is Player)
        {
            FSM.ChangeState("Wander");
        }
    }

    public void OnHealthComonentOnHealthChanged(float currentHealth)
    {
        HealthBar.Value = currentHealth / maxHealth;
    }

    public void OninputEvent(Node viewport, InputEvent @event, int shapeIdx)
    {
        if(@event is InputEventMouseButton mouseEvent && @event.IsPressed() && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            Refs.Instance.player.SelectedEnemy = this;
        }
    }

    public void OnHealthComponentOnDead()
    {
        if(Refs.Instance.player.SelectedEnemy == this)
        {
            Refs.Instance.player.SelectedEnemy = null;
        }
        DropLoot();
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnQuestProgressUpdated,"Enemy10",1);
        QueueFree();
    }
}
