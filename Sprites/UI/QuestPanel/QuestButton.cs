using Godot;
using System;

public partial class QuestButton : Button
{
    [Export]
    public Label QuestName { get; set;}
    [Export]
    public Label QuestDescription { get; set;}

    [Export]
    public TextureRect ItemIcon { get; set;}
    [Export]
    public Label QuestProgress { get; set;}

    public QuestData questData;
    public int currentProgress;

    public override void _Ready()
    {
        base._Ready();
        EventBus.Instance.OnQuestProgressUpdated += OnQuestProgressUpdated;
    }

    public void LoadData(QuestData data)
    {
        questData = data;
        QuestName.Text = data.questName;
        QuestDescription.Text = data.questDescription;
        ItemIcon.Texture = data.questItemReward.itemIron;
        UpdateProgressUi();
    }

    public void UpdateProgressUi()
    {
        if(currentProgress >= questData.questTargetValue)
        {
            currentProgress = questData.questTargetValue;
            SelfModulate = Colors.Green;
        }
        QuestProgress.Text = $"{currentProgress}/{questData.questTargetValue}";
    }

    public void OnQuestProgressUpdated(string questId, int amount)
    {
        if(questData != null && questData.questId == questId)
        {
            if(currentProgress < questData.questTargetValue)
            {
                currentProgress += amount;
                UpdateProgressUi();
            }
        }
    }

    public void OnPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        if(currentProgress >= questData.questTargetValue)
        {
            Inventory.Instance.AddItem(questData.questItemReward);
            QueueFree();
        }
    }
}
