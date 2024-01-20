using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 인벤토리에 넣을 수 있는 모든 아이템을 나타내는 ScripttableObject입니다.
/// </summary>
public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
{
    [Tooltip("저장 및 불러오기를 위한 자동 생성 UUID, 새 UUID를 생성하려면 이 필드를 지웁니다")]
    [SerializeField] private string itemID = null;
    [Tooltip("UI에 표시될 아이템 이름")]
    [SerializeField] private string displayName = null;
    [Tooltip("UI에 표시될 아이템 설명")]
    [SerializeField][TextArea] string description = null;
    [Tooltip("인벤토리에서 이 아이템을 나타내는 UI 아이콘")]
    [SerializeField] private Sprite icon;
    //[Tooltip("이 아이템이 드롭될 때 생성될 프리펩")]
    //[SerializeField] private 
    [Tooltip("창고 슬롯에 여러 아이템을 쌓을 수 있는지의 여부")]
    [SerializeField] private bool stackable = false;



    public bool isStackable() => stackable;
    public Sprite GetIcon() => icon;


    // 상태
    static Dictionary<string, InventoryItem> itemLookupCache;
    public void OnBeforeSerialize()
    {
        if(string.IsNullOrWhiteSpace(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
        }
    }

    public void OnAfterDeserialize()
    {
    }

    public string GetDisplayName() => displayName;

    public string GetDescription() => description;
}
