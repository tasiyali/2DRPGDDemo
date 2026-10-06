using Godot;
using System;

//道具类型
public enum E_Type
{
    //食物
    Food,
    //药水
    Potion,
    //卷轴
    Scroll,
    //装备
    Equipment,
    //技能
    Skill    
}

[GlobalClass]
public partial class Item : Resource
{
    [Export]
    public E_Type type;
    //是否可以丢弃
    [Export]
    public bool canDrop;
    //是否可以被消耗
    [Export]
    public bool isConsumable;
    //最大堆叠
    [Export]
    public int maxStack = 64;
    [Export]
    public string itemId;

}
