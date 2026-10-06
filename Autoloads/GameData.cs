using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GameData : Node
{
    public System.Collections.Generic.Dictionary<string,EquipData> equipment = new()
    {
        {"Helmet", null},
        {"Body", null},
        {"Legs", null},
        {"Weapon", null},
        {"Ring", null},
    };

    public Array<SkillData> skillSlot = new(){null, null, null, null, null};

    public float coins = 500.0f;

        // ✅ 单例模式
    private static GameData _instance;
    public static GameData Instance => _instance;
    
    public override void _EnterTree()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            QueueFree();
        }
    }
}
