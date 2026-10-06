using Godot;
using System;

public partial class DialoguePanel : Control
{
    [Export]
    public TextureRect NpcIcon { set; get; }
    [Export]
    public Label Label { set; get; }

    public DialogueData currentDialogueData;
    public int currentLineIndex = 0;
    public float typingSpeed = 0.05f; // 每个字符的打字速度，单位为秒
    public bool isTyping = false; // 是否正在打字
    public Tween tween; // Tween节点，用于实现打字效果

    public override void _Ready()
    {
        base._Ready();
        // 连接对话开始事件的信号
        EventBus.Instance.OnDialogueStarted += OnDialogueStarted;
        Hide(); // 初始时隐藏对话面板
    }

    public override void _Input(InputEvent @event)
    {
        if(!Visible) return; // 如果面板不可见，则不处理输入
        if(@event.IsAction("ui_accept") || (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left))
        {
            GetViewport().SetInputAsHandled(); // 阻止事件继续传递
            if(isTyping)
            {
                ComplateLine(); // 如果正在打字，则立即显示完整文本
            }
            else
            {
                NextLine(); // 如果打字已完成，则显示下一行文本
            }
        }
    }

    public void OnDialogueStarted(DialogueData dialogueData)
    {
        currentDialogueData = dialogueData;
        currentLineIndex = 0;
        NpcIcon.Texture = currentDialogueData.npcIcon;
        Show();
        ShowLine();
    }

    public void ComplateLine()
    {
        if(tween != null)
        {
            tween.Kill(); // 如果有正在进行的Tween，先停止它
        }
        //立即显示完整文本
        Label.VisibleCharacters = -1;
        isTyping = false; // 打字完成
    }

    public void NextLine()
    {
        currentLineIndex++;
        ShowLine();
    }

    public void ShowLine()
    {
        //如果当前行索引超出对话行数，则结束对话
        if(currentLineIndex >= currentDialogueData.dialogueLines.Count)
        {
            EndDialogue();
            return;
        }
        string line = currentDialogueData.dialogueLines[currentLineIndex];
        Label.Text = currentDialogueData.npcName + ":\n" + line;

        Label.VisibleCharacters = 0; // 重置可见字符数
        isTyping = true; // 开始打字
        
        if(tween != null)
        {
            tween.Kill(); // 如果有正在进行的Tween，先停止它
        }
        tween = CreateTween();
        float duration = Label.Text.Length * typingSpeed; // 计算打字总时间
        tween.TweenProperty(Label, "visible_characters", Label.Text.Length, duration);
        tween.Finished += () => { isTyping = false; }; // 打字完成后设置isTyping为false
    }

    public void EndDialogue()
    {
        Hide();
        currentDialogueData = null;
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnDialogueEnded);
    }
}
