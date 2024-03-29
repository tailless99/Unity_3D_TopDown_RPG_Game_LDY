using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 플레이어 인벤토리의 대한 저장 공간을 제공하는 코드,
/// 구성 가능한 갯수의 슬롯 사용
/// </summary>
public class Inventory : MonoBehaviour
{
    [Tooltip("허용된 인벤토리 크기")]
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
    /// 플레이어 게임 오브젝트에서 Inventory Component를 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public static Inventory GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<Inventory>();
    }

    /// <summary>
    /// 아이템을 첫 번째 사용 가능한 슬롯에 추가를 시도합니다.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public bool AddToFirstEmptySlot(InventoryItem item, int number)
    {
        int i = fintSlot(item);

        if (i < 0) return false;

        slots[i].item = item;
        slots[i].number += number; // 갯수증가

        inventoryUpdated?.Invoke();
        return true;

    }

    /// <summary>
    /// 이미 있는 슬롯에 아이템을 추가합니다
    /// 이 아이템의 유형이 이미 있는 경우, 스택에 추가합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <param name="number"></param>
    /// <exception cref="NotImplementedException"></exception>
    public bool addItemToSlot(int index, InventoryItem item, int number)
    {
        if (slots[index].item != null) return AddToFirstEmptySlot(item, number);

        // 해당 슬롯에 아이템을 추가하는 코드
        slots[index].item = item;
        slots[index].number += number;
        
        // 인벤토리 갱신
        inventoryUpdated?.Invoke();

        return true;
    }

    /// <summary>
    /// 주어진 아이템을 수용할 수 있는 슬롯을 찾습니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private int fintSlot(InventoryItem item)
    {
        // 쌓을 수 있는 슬롯을 먼저 찾는다.
        int i = FindStack(item);

        if(i < 0)
        {
            i = FindEmptySlot();
        }
        return i;
    }

    /// <summary>
    /// 빈 슬롯을 찾습니다.
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
    /// 이 아이템 유형에 기존 스택(아이템)을 찾는다.
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
