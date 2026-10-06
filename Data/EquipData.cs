using Godot;
using System;

//装备类型
public enum E_EquipType
{
    //头盔
    Helmet,
    //身体
    Body,
    //裤子
    Legs,
    //武器
    Weapon,
    //戒指
    Ring
}
[GlobalClass]
public partial class EquipData : ItemData
{
    [Export]
    public E_EquipType equipType;

    [Export]
    public float bounsDamage;
    [Export]
    public float bounsHealth;
    [Export]
    public float bounsSpeed;
    [Export]
    public float bounsMana;
    public EquipData()
    {
        type = E_Type.Equipment;
        maxStack = 1;
    }

    public string GetEquipKey()
    {
        switch (equipType)
        {
            case E_EquipType.Helmet:
                return "Helmet";
            case E_EquipType.Body:
                return "Body";
            case E_EquipType.Legs:
                return "Legs";
            case E_EquipType.Weapon:
                return "Weapon";
            case E_EquipType.Ring:
                return "Ring";
        }
        return "";
    }
}
