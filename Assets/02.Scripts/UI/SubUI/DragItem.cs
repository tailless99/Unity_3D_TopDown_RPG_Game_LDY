using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * IPointerEnterHandler : ���콺 �����Ͱ� UI ��� ���ο� ���� �� ȣ��ȴ�.
 * IPointerExitHandler : ���콺 �����Ͱ� UI ��ҿ��� ������ �� ȣ��ȴ�.
 * IBeginDragHandler : �巡�׸� ������ �� ȣ��ȴ�.
 * IDragHandler : �巡�� �� ������ ȣ��ȴ�.
 * IEndDragHandler : �巡�׸� ������ �� ȣ��ȴ�.
 * IDropHandler : �巡�׸� ���°� ���콺 ���� ��ư�� ������ ȣ��ǳ� EndDragHandler���� �ʰ� ȣ��ȴ�.
 */

/// <summary>
/// UI �������� �����̳ʷκ��� �巡�� �� ��ӵ� �� �ֵ��� �ϴ� Ŭ����
/// </summary>
public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler where T : class
{
    Vector3 startPosition; // �巡�� ������ġ
    Transform originalParent; // ���� �θ����
    IDragSource<T> source; // �巡���� ������ ����
    Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        source = GetComponentInParent<IDragSource<T>>();
    }

    // �巡�װ� ������ �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false; // �����ɽ�Ʈ ���� ����
        transform.SetParent(parentCanvas.transform, true); // true�� �θ�� ������ ī�޶� �������� �ٶ󺸵��� �ϴ� ��
    }

    public void OnDrag(PointerEventData eventData)
    {   // ���콺�� ��ġ���� ���� �������� �������� �ٲ۴�
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �ٽ� �ʱ�ȭ�Ѵ�.
        transform.position = startPosition; // ��ġ�� ���� ��������
        GetComponent<CanvasGroup>().blocksRaycasts = true; // �����ɽ�Ʈ ����
        transform.SetParent(originalParent, true); // �����θ�� ����

        IDragDestination<T> container;
        
        // UI���� ���� �ִ� ���� �ƴ� ���, ���� �������� �������
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            container = parentCanvas.GetComponent<IDragDestination<T>>();
        }
        else
        {
            container = GetConTainer(eventData);
        }

        if(container != null)
        {
            // �������� �Ѱ��ִ� �ڵ�
            DropItemIntoContainer(container);
        }
    }

    /// <summary>
    /// �����̳ʸ� �������� �޼ҵ�
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private IDragDestination<T> GetConTainer(PointerEventData eventData)
    {
        if (eventData.pointerEnter)
        {
            var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
            return container;
        }
        return null;
    }


    // �����̳ʷ� �������� ����ϴ� �޼ҵ�
    private void DropItemIntoContainer(IDragDestination<T> destination)
    {
        Debug.Log("DropItemSeccese");
        // ���� ������ �����Ǿ������� üũ�ϴ� ����
        if (object.ReferenceEquals(destination, source)) return;

        var destinationContainer = destination as IDragContainer<T>;
        var sourceContainer = source as IDragContainer<T>;

        // �������� ��ȯ�� �� ���� ��� => ��� ������ ����ִ� ���
        if(destinationContainer == null || sourceContainer == null || destinationContainer.GetItem() == null)
        {
            AttemptSimpleTrasfer(destination);
            return;
        }

    }

    private void AttemptSimpleTrasfer(IDragDestination<T> destination)
    {
        var draggingItem = source.GetItem();
        var draggingNumber = source.GetNumber();
        
        // ������ ���� ���ؼ� �ִ°�

        destination.AddItems(draggingItem, draggingNumber);
    }

    
}
