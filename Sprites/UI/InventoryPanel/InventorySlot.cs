using Godot;
using System;

public partial class InventorySlot : Button
{
    [Export]
    public TextureRect ItemIron { get; set;}
    [Export]
    public Label AmountLabel { get; set;}
    [Export]
    public TextureRect Selector { get; set;}

    public int slotIndex = -1;
    public Slotdata slotdata;

    public void LoadData(Slotdata data)
    {
        slotdata = data;

        if(slotdata != null && slotdata.item != null)
        {
            ItemIron.Texture = slotdata.item.itemIron;
            ItemIron.Show();

            if(slotdata.quantity > 1)
            {
                AmountLabel.Text = slotdata.quantity.ToString();
                AmountLabel.Show();
            }
            else
            {
                AmountLabel.Hide();
            }
        }
        else
        {
            ClearSlot();
        }
    }

    //在槽位没有物品时清除
    public void ClearSlot()
    {
        slotdata = null;
        ItemIron.Texture = null;
        ItemIron.Hide();
        AmountLabel.Hide();
    }

    public void OnMouseEntered()
    {
        Selector.Show();
    }

    public void OnMouseExited()
    {
        Selector.Hide();
    }

    public void OnGuiInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton mb && mb.IsPressed())
        {
            int buttonIndex = (int)mb.ButtonIndex;
            EventBus.Instance.EmitSignal(EventBus.SignalName.OnSlotClicked, slotIndex, buttonIndex);
        }
    }
}
