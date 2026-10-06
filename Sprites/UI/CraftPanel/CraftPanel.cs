using Godot;
using Godot.Collections;
using System;

public partial class CraftPanel : Control
{
    [Export]
    public Array<CraftData> craftRecipes;

    [Export]
    public GridContainer Container { get; set;}
    [Export]
    public VBoxContainer VBoxContainer { get; set;}

    [Export]
    public TextureRect ItemIcon { get; set;}
    [Export]
    public Label ItemName { get; set;}

    [Export]
    public TextureRect Material1Icon { get; set;}
    [Export]
    public Label Material1Name { get; set;}
    [Export]
    public Label Material1Qty { get; set;}
    [Export]
    public TextureRect Material2Icon { get; set;}
    [Export]
    public Label Material2Name { get; set;}
    [Export]
    public Label Material2Qty { get; set;}

    [Export]
    public Label AmountLabel { get; set;}

    public int amountSelected = 1;
    public CraftButton selectedButton;

    public override void _Ready()
    {
        base._Ready();
        VBoxContainer.Hide();
        foreach(Node child in Container.GetChildren())
        {
            child.QueueFree();
        }
        CreateCraftRecipes();
    }

    public void CreateCraftRecipes()
    {
        foreach(CraftData data in craftRecipes)
        {
            CraftButton craftButton = Refs.craftButton.Instantiate<CraftButton>();
            craftButton.Pressed += () => OnCraftButtonPressed(craftButton);
            Container.AddChild(craftButton);
            craftButton.LoadData(data);
        }
    }

    public void UpdateMaterialInformation()
    {
        ItemIcon.Texture = selectedButton.data.craftItem.itemIron;
        ItemName.Text = selectedButton.data.craftItem.itemName;
        AmountLabel.Text = amountSelected.ToString();

        CraftMaterial mat1 = selectedButton.data.craftMaterial[0];
        Material1Icon.Texture = mat1.item.itemIron;
        Material1Name.Text = mat1.item.itemName;
        int req1 = mat1.amount * amountSelected;
        Material1Qty.Text = $"{req1}/{Inventory.Instance.CountItem(mat1.item)}";

        CraftMaterial mat2 = selectedButton.data.craftMaterial[1];
        Material2Icon.Texture = mat2.item.itemIron;
        Material2Name.Text = mat2.item.itemName;
        int req2 = mat2.amount * amountSelected;
        Material2Qty.Text = $"{req2}/{Inventory.Instance.CountItem(mat2.item)}";
    }

    public bool CanCraftItem()
    {
        CraftMaterial mat1 = selectedButton.data.craftMaterial[0];
        CraftMaterial mat2 = selectedButton.data.craftMaterial[1];
        int req1 = mat1.amount * amountSelected;
        int req2 = mat2.amount * amountSelected;

        return Inventory.Instance.CountItem(mat1.item) >= req1 && Inventory.Instance.CountItem(mat2.item) >= req2;
    }

    public void OnCraftButtonPressed(CraftButton craftButton)
    {
        if(!VBoxContainer.Visible)
        {
            VBoxContainer.Show();
        }
        SoundManager.Instance.Play((int)E_Sound.Button);
        selectedButton = craftButton;
        amountSelected = 1;
        UpdateMaterialInformation();
    }

    public void OnRemoveButtonPressed()
    {
        amountSelected -= 1;
        amountSelected = Math.Max(1, amountSelected);
        UpdateMaterialInformation();
        SoundManager.Instance.Play((int)E_Sound.Button);
    }

    public void OnPlusButtonPressed()
    {
        amountSelected += 1;
        UpdateMaterialInformation();
        SoundManager.Instance.Play((int)E_Sound.Button);
    }

    public void OnCraftButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        if (!CanCraftItem())
        {
            return;
        }
        CraftMaterial mat1 = selectedButton.data.craftMaterial[0];
        CraftMaterial mat2 = selectedButton.data.craftMaterial[1];

        Inventory.Instance.RemoveItem(mat1.item, mat1.amount * amountSelected);
        Inventory.Instance.RemoveItem(mat2.item, mat2.amount * amountSelected);
        Inventory.Instance.AddItem(selectedButton.data.craftItem, amountSelected);

        amountSelected = 1;
        UpdateMaterialInformation();
    }

    public void OnCloseButtonPressed()
    {
        SoundManager.Instance.Play((int)E_Sound.Button);
        Hide();
    }
}
