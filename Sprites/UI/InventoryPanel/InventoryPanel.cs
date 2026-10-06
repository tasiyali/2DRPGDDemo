using Godot;
using System;
using Godot.Collections;

public partial class InventoryPanel : PanelContainer
{
    [Export]
    public GridContainer Container { get; set;}
    [Export]
    public Label CoinAmount { get; set;}
    [Export]
    public InventorySlot GrabbedSlot { get; set;}

    public Array<InventorySlot> slots = new();
    public int selectedSlotIndex = -1;

    public override void _Ready()
    {
        base._Ready();
        EventBus.Instance.OnInventoryChanged += OnInventoryChanged;
        EventBus.Instance.OnSlotClicked += OnSlotClicked;
        for(int i = 0; i < Container.GetChildCount(); i++)
        {
            InventorySlot slot = Container.GetChild<InventorySlot>(i);
            
            slot.slotIndex = i;
            slots.Add(slot);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        CoinAmount.Text = GameData.Instance.coins.ToString();
        if (GrabbedSlot.Visible)
        {
            GrabbedSlot.GlobalPosition = GetGlobalMousePosition();
        }
    }

    public void HandLeftButton(int index)
    {
        if(selectedSlotIndex >= 0 && selectedSlotIndex != index)
        {
            ItemData fromItem = Inventory.Instance.GetSlotItem(selectedSlotIndex);
            ItemData toItem = Inventory.Instance.GetSlotItem(index);
            

            if(fromItem != null && toItem != null && fromItem == toItem)
            {
                Inventory.Instance.MergeSlot(selectedSlotIndex, index);
            }
            else
            {
                Inventory.Instance.SwapSlots(selectedSlotIndex, index);
            }
            DeselectSlot();
        }
        else
        {
            if(selectedSlotIndex == index)
            {
                DeselectSlot();
            }
            else
            {
                if(Inventory.Instance.GetSlot(index) != null)
                {
                    SelectSlot(index);
                }
            }
        }
    }

    public void HandRightButton(int index)
    {
        Item item = Inventory.Instance.GetSlotItem(index);
        if(item == null)
        {
            return;
        }

        if(item is EquipData)
        {
            Inventory.Instance.EquipItem(index);
        }
        else
        {
            Inventory.Instance.UseItem(index);
            EventBus.Instance.EmitSignal(EventBus.SignalName.OnInventoryUsedItem, item);
        }

    }

    public void SelectSlot(int slotIndex)
    {
        DeselectSlot();
        selectedSlotIndex = slotIndex;

        Slotdata slot = Inventory.Instance.GetSlot(slotIndex);
        GrabbedSlot.LoadData(slot);
        GrabbedSlot.Show();
    }

    public void DeselectSlot()
    {
        selectedSlotIndex = -1;
        GrabbedSlot.Hide();
    }

    public void OnInventoryChanged()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            Slotdata slotData = Inventory.Instance.GetSlot(i);
            slots[i].LoadData(slotData);
        }
    }

    public void OnSlotClicked(int index, int button)
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        switch ((MouseButton)button)
        {
            case MouseButton.Left:
                HandLeftButton(index);
                break;
            case MouseButton.Right:
                HandRightButton(index);
                break;
        }
    }
}
