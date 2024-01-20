using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] InventoryItemIcon icon;

    // STATE
    int index;
    InventoryItem item;
    Inventory Inventory;

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
