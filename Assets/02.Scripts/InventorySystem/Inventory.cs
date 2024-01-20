using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �÷��̾� �κ��丮�� ���� ���� ������ �����ϴ� �ڵ�,
/// ���� ������ ������ ���� ���
/// </summary>
public class Inventory : MonoBehaviour
{
    [Tooltip("���� �κ��丮 ũ��")]
    [SerializeField] private int inventorySize = 16;
    [SerializeField] InventorySlot[] slots;

    [Serializable]
    public struct InventorySlot
    {
        public InventoryItem item;
        public int number;
    }

    public event Action inventoryUpdated;

    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
    }

    public int GetSize() => slots.Length;


    /// <summary>
    /// �÷��̾� ���� ������Ʈ���� Inventory Component�� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public static Inventory GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<Inventory>();
    }

    /// <summary>
    /// �������� ù ��° ��� ������ ���Կ� �߰��� �õ��մϴ�.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public bool AddToFirstEmptySlot(InventoryItem item, int number)
    {
        int i = fintSlot(item);

        if (i < 0) return false;

        slots[i].item = item;
        slots[i].number += number; // ��������

        inventoryUpdated?.Invoke();
        return true;

    }

    /// <summary>
    /// �־��� �������� ������ �� �ִ� ������ ã���ϴ�.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private int fintSlot(InventoryItem item)
    {
        // ���� �� �ִ� ������ ���� ã�´�.
        int i = FindStack(item);

        if(i < 0)
        {
            i = FindEmptySlot();
        }
        return i;
    }

    /// <summary>
    /// �� ������ ã���ϴ�.
    /// </summary>
    /// <returns></returns>
    private int FindEmptySlot()
    {
        for(int i=0; i<slots.Length; i++)
        {
            if (slots[i].item == null) return i;
        }
        return -1;
    }

    /// <summary>
    /// �� ������ ������ ���� ����(������)�� ã�´�.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private int FindStack(InventoryItem item)
    {
        if(!item.isStackable())
        {
            return -1;
        }

        for(int i=0; i<slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i], item)) return i;
        }

        return -1;
    }

    public InventoryItem GetItemInSlot(int index)
    {
        return slots[index].item;
    }

    public int GetNumberInSlot(int index)
    {
        return slots[index].number;
    }
}
