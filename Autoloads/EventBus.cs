using Godot;
using System;


/// <summary>
/// 事件总线类，用于在游戏中传递事件和信号
/// </summary>
public partial class EventBus : Node
{

    //声明一个信号，当玩家创建时触发
    [Signal]
    public delegate void OnPlayerCreatedEventHandler();

    //声明一个信号，当玩家生命值更新时触发
    [Signal]
    public delegate void OnPlayerHealthUpdatedEventHandler(float currentHealth, float maxHealth);

    //声明一个信号，当玩家魔法值更新时触发
    [Signal]
    public delegate void OnPlayerManaUpdatedEventHandler(float currentMana, float maxMana);

    //声明一个信号，当玩家经验值更新时触发
    [Signal]
    public delegate void OnPlayerExpUpdatedEventHandler(float currentExp, float nextLevelExp);

    //声明一个信号，当玩家状态更新时触发
    [Signal]
    public delegate void OnPlayerStateUpdatedEventHandler();

    //声明一个信号，当槽位更新时触发
    [Signal]
    public delegate void OnInventoryChangedEventHandler();

    //声明一个信号，当槽位点击时触发
    [Signal]
    public delegate void OnSlotClickedEventHandler(int index, int button);

    //声明一个信号，当道具被使用时触发
    [Signal]
    public delegate void OnInventoryUsedItemEventHandler(ItemData item);
    
    //声明一个信号，当装备更新时触发
    [Signal]
    public delegate void OnEquipmentChangedEventHandler();

    //声明一个信号，当敌人死亡时触发
    [Signal]
    public delegate void OnEnemyDiedEventHandler();

    //声明一个信号，当对话开始时触发
    [Signal]
    public delegate void OnDialogueStartedEventHandler(DialogueData dialogueData);
    //声明一个信号，当对话结束时触发
    [Signal]
    public delegate void OnDialogueEndedEventHandler();

    [Signal]
    public delegate void OnQuestProgressUpdatedEventHandler(string questId, int amount);

    // 单例实例
    private static EventBus _instance;
    public static EventBus Instance => _instance;
    public override void _EnterTree()
    {
        if (_instance == null)
        {
            _instance = this;  // ✅ 这里给 _instance 赋值！
        }
        else
        {
            QueueFree();  // 防止重复实例
        }
    }
}
