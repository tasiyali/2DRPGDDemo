using Godot;
using System;

public partial class EquippedSkillButton : Button
{
    [Export]
    public int number;
    [Export]
    public Panel Panel { get; set; }
    [Export]
    public TextureRect SkillIcon { get; set; }
    [Export]
    public Label Label { get; set; }

    public SkillData equippedData;

    public override void _Ready()
    {
        base._Ready();
        Label.Text = number.ToString();
    }

    public void EquipSkill(SkillData data)
    {
        equippedData = data;
        Panel.Hide();
        SkillIcon.Texture = data.itemIron;
        SkillIcon.Show();

    }

    public void ResetSkillButton()
    {
        Panel.Show();
        SkillIcon.Hide();
    }

    public void OnPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        if(equippedData != null)
        {
            GameData.Instance.skillSlot[number - 1] = null;
            equippedData = null;
            ResetSkillButton();
        }
    }
}
