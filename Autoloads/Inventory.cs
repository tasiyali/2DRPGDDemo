using Godot;
using System;
using Godot.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

public partial class Inventory : Node
{
    public const int slotSize = 30;

    public Array<Slotdata> inventory = new();

    public override void _Ready()
    {
        base._Ready();
        inventory.Clear();
        inventory.Resize(slotSize);
    }

    // public  void _input(InputEvent @event)
    // {
    //     if (@event.IsActionPressed("ui_accept"))
    //     {
    //         AddItem(GD.Load<ItemData>("uid://dcgtydbr2yy1j"), 5);
    //     }
    // }


    #region 移动

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if(itemData == null)
        {
            return;
        }
        int remaining = amount;

        //为物品进行堆叠
        if(itemData.maxStack > 1)
        {
            foreach(int index in FindItemIndexes(itemData, true))
            {
                if(remaining <= 0)
                {
                    break;
                }
                Slotdata slotdata = inventory[index];
                int space = itemData.maxStack - slotdata.quantity;
                int toGive = Mathf.Min(space, remaining);

                slotdata.quantity += toGive;
                remaining -= toGive;
            }
        }

        //为多余物品存入其他槽位
        if(remaining > 0)
        {
            foreach(int index in GetEmptySlotsIndexed())
            {
                if(remaining <= 0)
                {
                    break;
                }

                int toGive = Mathf.Min(itemData.maxStack, remaining);
                inventory[index] = new Slotdata(itemData, toGive);
                remaining -= toGive;
            }
        }
        int added = amount - remaining;
        if(added > 0)
        {
            EventBus.Instance.EmitSignal(EventBus.SignalName.OnInventoryChanged);
        }
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        int remaining = amount;
        Array<int> itemIndexes = FindItemIndexes(item);
        itemIndexes.Reverse();

        foreach(int index in itemIndexes)
        {
            if(remaining <= 0)
            {
                break;
            }
            Slotdata slot = inventory[index];
            int take = Math.Min(slot.quantity, remaining);
            slot.quantity -= take;
            remaining -= take;

            if(slot.quantity <= 0)
            {
                inventory[index] = null;
            }
        }

        int removed =amount - remaining;
        if(removed > 0)
        {
            EventBus.Instance.EmitSignal(EventBus.SignalName.OnInventoryChanged);
        }
    }

    #endregion

    #region 查找

    //获得空槽位的索引值
    public Array<int> GetEmptySlotsIndexed()
    {
        Array<int> empty = new();
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i] == null)
            {
                empty.Add(i);
            }
        }
        return empty;
    }

    //查找一个物品并返回它的索引值
    public Array<int> FindItemIndexes(ItemData itemData, bool withSpace = false)
    {
        Array<int> found = new();
        for(int i = 0; i < inventory.Count; i++)
        {
            Slotdata slotdata = inventory[i];
            if(slotdata != null && slotdata.item == itemData)
            {
                if (withSpace)
                {
                    if(slotdata.quantity < itemData.maxStack)
                    {
                        found.Add(i);
                    }
                }
                else
                {
                    found.Add(i);
                }
            }
        }
        return found;
    }

    public Slotdata GetSlot(int index)
    {
        if(index >= 0 && index < inventory.Count)
        {
            return inventory[index];
        }
        return null;
    }

    public ItemData GetSlotItem(int index)
    {
        Slotdata slot = GetSlot(index);
        if(slot != null)
        {
            return slot.item;
        }
        return null;
    }

    public int CountItem(ItemData item)
    {
        int total = 0;
        foreach(Slotdata slot in inventory)
        {
            if(slot != null && slot.item == item)
            {
                total += slot.quantity;
            }
        }

        return total;
    }

    #endregion

    #region 移动物品

    //交换物品
    public void SwapSlots(int fromIndex , int toIndex)
    {
        if(fromIndex < 0 && fromIndex >= inventory.Count)
        {
            return;
        }
        if(toIndex < 0 && toIndex >= inventory.Count)
        {
            return;
        }
        Slotdata temp = inventory[fromIndex];
        inventory[fromIndex] = inventory[toIndex];
        inventory[toIndex] = temp;
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnInventoryChanged);
    }

    //合并物品
    public void MergeSlot(int fromIndex , int toIndex)
    {
        Slotdata fromSlot = GetSlot(fromIndex);
        Slotdata toSolt = GetSlot(toIndex);

        if(fromSlot == null || toSolt == null)
        {
            return;
        }
        if(fromSlot.item != toSolt.item)
        {
            return;
        }

        Item item = fromSlot.item;
        if(item.maxStack <= 1)
        {
            return;
        }

        int space = item.maxStack - toSolt.quantity;
        int toMove = Mathf.Min(space, fromSlot.quantity);

        toSolt.quantity += toMove;
        fromSlot.quantity -= toMove;

        if(fromSlot.quantity <= 0)
        {
            inventory[fromIndex] = null;
        }
        else if(space <= 0)
        {
            //没有空间就交换道具
            SwapSlots(fromIndex, toIndex);
            return;
        }
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnInventoryChanged);
    }

    #endregion
    
    #region 使用道具

    public void UseItem(int index)
    {
        Slotdata slot = GetSlot(index);
        if(slot == null)
        {
            return;
        }
        if (!slot.item.isConsumable)
        {
            return;
        }
        slot.quantity -= 1;
        if(slot.quantity <= 0)
        {
            inventory[index] = null;
        }
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnInventoryChanged);
    }

    public void EquipItem(int index)
    {
        Slotdata slot = GetSlot(index);
        if(slot == null)
        {
            return;
        }
        if(!(slot.item is EquipData))
        {
            return;
        }

        EquipData equipData = slot.item as EquipData;
        string equipKey = equipData.GetEquipKey();

        //保存装备
        EquipData currentEquipped = GameData.Instance.equipment[equipKey];

        //装备新装备
        GameData.Instance.equipment[equipKey] = equipData;

        //清除背包槽
        inventory[index] = null;

        //把已经装备的装备放回背包里
        if(currentEquipped != null)
        {
            AddItem(currentEquipped, 1);
        }
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnInventoryChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnEquipmentChanged);
    }

    //卸除装备
    public void UnequipItem(E_EquipType equipType)
    {
        string equipKey = equipType.ToString();
        EquipData equippedItem = GameData.Instance.equipment[equipKey];

        if(equippedItem == null)
        {
            return;
        }

        //放到背包里
        AddItem(equippedItem, 1);

        //清除装备栏上的装备
        GameData.Instance.equipment[equipKey] = null;

        EventBus.Instance.EmitSignal(EventBus.SignalName.OnEquipmentChanged);
    }

    #endregion
    // ✅ 单例模式
    private static Inventory _instance;
    public static Inventory Instance => _instance;
    
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
