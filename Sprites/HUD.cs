using Godot;
using Godot.Collections;
using System;

public partial class HUD : CanvasLayer
{
    [Export]
    public Array<EquippedSkillButton> SkillButtons { get; set; }

    [Export]
    public EquipmentPanel EquipmentPanel { get; set; }
    [Export]
    public InventoryPanel InventoryPanel { get; set; }
    [Export]
    public StatsPanel StatsPanel { get; set; }
    [Export]
    public SkillPanel SkillPanel { get; set; }
    [Export]
    public DialoguePanel DialoguePanel { get; set; }
    [Export]
    public ShopPanel ShopPanel { get; set;}
    [Export]
    public CraftPanel CraftPanel { get; set;}
    [Export]
    public QuestPanel QuestPanel { get; set;}

    [Export]
    public ProgressBar HealthBar { get; set; } 
    [Export]
    public ProgressBar ManaBar { get; set; }
    [Export]
    public ProgressBar ExpBar { get; set; }

    [Export]
    public Label HPLabel { get; set; }
    [Export]
    public Label ManaLabel { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Refs.Instance.hUD = this;
        // 连接事件总线的信号到HUD的处理方法
        EventBus.Instance.OnPlayerHealthUpdated += OnPlayerHealthUpdated;
        EventBus.Instance.OnPlayerManaUpdated += OnPlayerManaUpdated;
        EventBus.Instance.OnPlayerExpUpdated += OnPlayerExpUpdated;
    }

    public void EquipSkillToSlot(SkillData skill)
    {
        // 遍历技能按钮，找到第一个未装备技能的按钮，并将技能装备到该按钮上
        foreach(EquippedSkillButton button in SkillButtons)
        {
            if(button.equippedData == null)
            {
                button.EquipSkill(skill);
                GameData.Instance.skillSlot[button.number - 1] = skill;
                break;
            }
        }
    }

    public void OpenNpcPanel(E_NpcType type)
    {
        switch (type)
        {
            case E_NpcType.Shop:
                ShopPanel.Show();
                break;
            case E_NpcType.Craft:
                CraftPanel.Show();
                break;
            case E_NpcType.Quest:
                QuestPanel.Show();
                break;
        }
    }

    public void OnEquimentButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        EquipmentPanel.Visible = !EquipmentPanel.Visible;
    }

    public void OnInventoryButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        InventoryPanel.Visible = !InventoryPanel.Visible;
    }

    public void OnStatsButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        StatsPanel.Visible = !StatsPanel.Visible;
    }

    public void OnSkillsButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        SkillPanel.Visible = !SkillPanel.Visible;
    }

    public void OnPlayerHealthUpdated(float currentHealth, float maxHealth)
    {
        HealthBar.Value = currentHealth / maxHealth;
        HPLabel.Text = $"{currentHealth}/{maxHealth}";
    }

    public void OnPlayerManaUpdated(float currentMana, float maxMana)
    {
        ManaBar.Value = currentMana / maxMana;
        ManaLabel.Text = $"{currentMana}/{maxMana}";
    }

    public void OnPlayerExpUpdated(float currentExp, float nextLevelExp)
    {
        ExpBar.Value = currentExp / nextLevelExp;
    }
}
