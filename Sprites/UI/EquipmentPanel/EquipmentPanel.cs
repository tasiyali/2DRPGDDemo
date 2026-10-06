using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class EquipmentPanel : PanelContainer
{
    [Export]
    public Label DamageAmount { get; set;}

    public Array<EquipmentSlot> slots = new();
    public override void _Ready()
    {
        base._Ready();
        slots = new Array<EquipmentSlot>
        {
            GetNode<EquipmentSlot>("%HelmetSlot"),
            GetNode<EquipmentSlot>("%BodySlot"),
            GetNode<EquipmentSlot>("%LegsSlot"),
            GetNode<EquipmentSlot>("%WeaponSlot"),
            GetNode<EquipmentSlot>("%RingSlot"),
        };
        EventBus.Instance.OnEquipmentChanged += OnEquipmentChanged;
        foreach(EquipmentSlot slot in slots)
        {
            slot.Pressed += () => OnButtonPressed(slot);   
        }
    }

    public void OnEquipmentChanged()
    {
        var items = GameData.Instance.equipment.Values.ToList();
        float equippedDmg = 0.0f;
        for(int i = 0; i < slots.Count; i++)
        {
            EquipData equipItem = items[i];
            slots[i].LoadData(equipItem);

            if(equipItem != null)
            {
                equippedDmg += equipItem.bounsDamage;
            }
        }
        DamageAmount.Text = equippedDmg.ToString();

        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerStateUpdated);
    }

    public void OnButtonPressed(EquipmentSlot slot)
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        Inventory.Instance.UnequipItem(slot.equipType);
    }
}
