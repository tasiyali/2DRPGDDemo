using Godot;
using System;

public partial class SkillButton : Button
{
    [Export]
    public bool isFree;
    [Export]
    public SkillData skillData;

    [Export]
    public TextureRect SkillIcon { get; set;}
    [Export]
    public TextureRect Lock { get; set;}

    public SkillData skill;
    public bool isUnlocked = false;

    public override void _Ready()
    {
        base._Ready();
        EnableSkill(false);

        if(skillData != null)
        {
            LoadData(skillData);
        }
        if(isFree)
        {
            EnableSkill(true);
        }
    }

    public void LoadData(SkillData data)
    {
        skill = data;
        SkillIcon.Texture = data.itemIron;
    }

    public void EnableSkill(bool value)
    {
        SkillIcon.SelfModulate = value ? Colors.White : new Color("5b5b5b");
        Lock.Visible = !value;
    }

    public void OnPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        if(!isUnlocked)
        {
            if(GameData.Instance.coins >= skillData.itemPrice)
            {
                GameData.Instance.coins -= skillData.itemPrice;
                isUnlocked = true;
                EnableSkill(true);
            }
        }
        else
        {
            if(Lock.Visible)
            {
                EnableSkill(true);
            }
            else
            {
                Refs.Instance.hUD.EquipSkillToSlot(skillData);
            }
        }
    }
}
