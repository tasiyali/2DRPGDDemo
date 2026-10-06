using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public enum E_NpcType
{
    Idle,
    Shop,
    Quest,
    Craft
}

public partial class NpcSimple : Area2D
{
    [Export]
    public E_NpcType type;
    [Export]
    public bool canMove;
    [Export]
    public float moveSpeed = 50.0f;
    [Export]
    public float waitTime = 2.0f;
    [Export]
    public DialogueData dialogueData;

    [Export]
    public AnimatedSprite2D Sprite2D { set; get; }
    [Export]
    public NavigationAgent2D NavigationAgent { set; get; }
    [Export]
    public Timer timer { set; get; }

    public string lastDirection = "down";

    public override async void _Ready()
    {
        base._Ready();
        if(!canMove)
        {
            return;
        }
        
        await ToSignal(GetTree(), "process_frame"); // 等待一帧，确保导航网格已加载
        SetNewTarget();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!canMove)
        {
            return;
        }

        if (IsWaitingForNextMove())
        {
            PlayAnimation("idle");
            return;
        }

        if (HasReachedTarget())
        {
            if (timer.IsStopped())
            {
                timer.Start(waitTime);
            }
            return;
        }

        Vector2 nextPathPos = NavigationAgent.GetNextPathPosition();
        Vector2 direction = GlobalPosition.DirectionTo(nextPathPos);
        GlobalPosition += direction * moveSpeed * (float)delta;
        UpDateDirection(direction);
        PlayAnimation("walk");
    }

    public async void OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
    {
        if(@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if(dialogueData != null)
            {
                EventBus.Instance.EmitSignal(EventBus.SignalName.OnDialogueStarted, dialogueData);
                if(type != E_NpcType.Idle)
                {
                    await ToSignal(EventBus.Instance, EventBus.SignalName.OnDialogueEnded);
                    Refs.Instance.hUD.OpenNpcPanel(type);
                }
            }
            else
            {
                if(type != E_NpcType.Idle)
                {
                    Refs.Instance.hUD.OpenNpcPanel(type);
                }
            }
        }
    }

    public void SetNewTarget()
    {
        if(Refs.Instance.navigation == null)
        {
            return;
        }

        // 获取导航网格中所有已使用的单元格
        Array<Vector2I> usedCells = Refs.Instance.navigation.GetUsedCells();
        if(usedCells.Count == 0)
        {
            return;
        }

        // 从已使用的单元格中随机选择一个作为新的目标位置
        Vector2I randomCell = usedCells.PickRandom();
        // 将单元格坐标转换为世界坐标
        Vector2 worldPos = Refs.Instance.navigation.ToGlobal(Refs.Instance.navigation.MapToLocal(randomCell));
        NavigationAgent.TargetPosition = worldPos;
    }

    public bool IsWaitingForNextMove()
    {
        return !timer.IsStopped();
    }

    public bool HasReachedTarget()
    {
        return NavigationAgent.IsNavigationFinished();
    }

    public void PlayAnimation(string anim)
    {
        Sprite2D.Play($"{anim}_{lastDirection}");
    }

    public void UpDateDirection(Vector2 dir)
    {
        if(Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))
        {
            lastDirection = dir.X > 0 ? "right" : "left";
        }
        else
        {
            lastDirection = dir.Y > 0 ? "down" : "up";
        }
    }

    public void OnTimerout()
    {
        SetNewTarget();
    }
}
