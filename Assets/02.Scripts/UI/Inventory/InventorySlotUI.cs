using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
{
    [SerializeField] InventoryItemIcon icon;

    // STATE
    int index;
    InventoryItem item;
    Inventory Inventory;
    

    public InventoryItem GetItem()
    {
        return Inventory.GetItemInSlot(index);
    }

    public int GetNumber()
    {
        return Inventory.GetNumberInSlot(index);
    }

    public void AddItems(InventoryItem item, int number)
    {
        // TODO
        // �κ��丮�� �������� ����ϴ� �ڵ�
        Inventory.addItemToSlot(index, item, number);
    }

    public int MaxAcceptable(InventoryItem item)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveItems(int number)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// �κ��丮���� �ش� ���Կ� �ִ� �����۰� ���� �ε����� �޾ƿ��� �ڵ�
    /// </summary>
    /// <param name="inventory"></param>
    /// <param name="index"></param>
    public void Setup(Inventory inventory, int index)
    {
        this.Inventory = inventory;
        this.index = index;
        icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
    }
}
