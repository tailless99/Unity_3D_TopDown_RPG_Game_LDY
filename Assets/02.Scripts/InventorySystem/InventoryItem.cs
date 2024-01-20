using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �κ��丮�� ���� �� �ִ� ��� �������� ��Ÿ���� ScripttableObject�Դϴ�.
/// </summary>
public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
{
    [Tooltip("���� �� �ҷ����⸦ ���� �ڵ� ���� UUID, �� UUID�� �����Ϸ��� �� �ʵ带 ����ϴ�")]
    [SerializeField] private string itemID = null;
    [Tooltip("UI�� ǥ�õ� ������ �̸�")]
    [SerializeField] private string displayName = null;
    [Tooltip("UI�� ǥ�õ� ������ ����")]
    [SerializeField][TextArea] string description = null;
    [Tooltip("�κ��丮���� �� �������� ��Ÿ���� UI ������")]
    [SerializeField] private Sprite icon;
    //[Tooltip("�� �������� ��ӵ� �� ������ ������")]
    //[SerializeField] private 
    [Tooltip("â�� ���Կ� ���� �������� ���� �� �ִ����� ����")]
    [SerializeField] private bool stackable = false;



    public bool isStackable() => stackable;
    public Sprite GetIcon() => icon;


    // ����
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
