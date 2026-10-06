using Godot;
using System;

[GlobalClass]
public partial class QuestData : Resource
{
    [Export]
    public string questId;
    [Export]
    public string questName;
    [Export(PropertyHint.MultilineText)]
    public string questDescription;
    [Export]
    public ItemData questItemReward;
    [Export]
    public int questTargetValue;
}
