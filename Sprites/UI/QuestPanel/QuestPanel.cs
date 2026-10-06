using Godot;
using Godot.Collections;
using System;

public partial class QuestPanel : Control
{
    [Export]
    public Array<QuestData> quests;
    [Export]
    public VBoxContainer Container { get; set;}

    public override void _Ready()
    {
        base._Ready();
        foreach(Node child in Container.GetChildren())
        {
            child.QueueFree();
        }
        CreateQuestButton();
    }

    public void CreateQuestButton()
    {
        foreach(QuestData data in quests)
        {
            QuestButton questButton = Refs.questButton.Instantiate<QuestButton>();
            Container.AddChild(questButton);
            questButton.LoadData(data);
        }
    }

    public void OnCloseButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        Hide();
    }
}
