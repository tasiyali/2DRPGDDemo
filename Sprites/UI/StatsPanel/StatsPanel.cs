using Godot;
using System;

public partial class StatsPanel : PanelContainer
{
    [Export]
    public Label DamageLabel { get; set; }
    [Export]
    public Label HPLabel { get; set; }
    [Export]
    public Label VelLabel { get; set; }
    [Export]
    public Label ManaLabel { get; set; }
    [Export]
    public Label CritLabel { get; set; }
    [Export]
    public Label CritDMGLabel { get; set; }
    [Export]
    public Label CurrentLevelLabel { get; set; }
    [Export]
    public Label CurrentPointsLabel { get; set; }
    [Export]
    public Label STRPointsLabel { get; set; }
    [Export]
    public Label DEXPointsLabel { get; set; }
    [Export]
    public Label INTPointsLabel { get; set; }

    public override void _Ready()
    {
        base._Ready();
        EventBus.Instance.OnPlayerCreated += OnPlayerCreated;
        EventBus.Instance.OnPlayerStateUpdated += OnPlayerStateUpdated;
    }

    public void UpdateStats()
    {
        if(!IsInstanceValid(Refs.Instance.player))
        {
            return;
        }

        DamageLabel.Text = $"伤害: {Refs.Instance.player.AttackPower}";
        HPLabel.Text = $"生命: {Refs.Instance.player.maxHealth}";
        VelLabel.Text = $"速度: {Refs.Instance.player.moveSpeed}";
        ManaLabel.Text = $"魔力: {Refs.Instance.player.maxMana}";
        CritLabel.Text = $"暴击: {Refs.Instance.player.critChance}%";
        CritDMGLabel.Text = $"暴击伤害: {Refs.Instance.player.critDamage}%";

        CurrentLevelLabel.Text = $"等级 {Refs.Instance.player.currentLevel}";
        CurrentPointsLabel.Text = $"技能点数: {Refs.Instance.player.currentPoints}";

        STRPointsLabel.Text = Refs.Instance.player.strValue.ToString();
        DEXPointsLabel.Text = Refs.Instance.player.dexValue.ToString();
        INTPointsLabel.Text = Refs.Instance.player.intValue.ToString();
    }

    public void OnPlayerCreated()
    {
        UpdateStats();
    }

    public void OnPlayerStateUpdated()
    {
        UpdateStats();
    }

    public void OnStrButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        Refs.Instance.player.UpdateStat("STR");
    }

    public void OnDexButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        Refs.Instance.player.UpdateStat("DEX");
    }

    public void OnIntButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        Refs.Instance.player.UpdateStat("INT");
    }
}
