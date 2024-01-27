using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * IPointerEnterHandler : 마우스 포인터가 UI 요소 내부에 들어갔을 때 호출된다.
 * IPointerExitHandler : 마우스 포인터가 UI 요소에서 나왔을 때 호출된다.
 * IBeginDragHandler : 드래그를 시작할 때 호출된다.
 * IDragHandler : 드래그 할 때마다 호출된다.
 * IEndDragHandler : 드래그를 끝냈을 때 호출된다.
 * IDropHandler : 드래그를 끝냈고 마우스 왼쪽 버튼을 땠을때 호출되나 EndDragHandler보다 늦게 호출된다.
 */

/// <summary>
/// UI 아이템이 컨테이너로부터 드래그 앤 드롭될 수 있도록 하는 클래스
/// </summary>
public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler where T : class
{
    Vector3 startPosition; // 드래그 시작위치
    Transform originalParent; // 원래 부모계층
    IDragSource<T> source; // 드래그할 아이템 정보
    Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        source = GetComponentInParent<IDragSource<T>>();
    }

    // 드래그가 시작할 때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false; // 레이케스트 차단 해제
        transform.SetParent(parentCanvas.transform, true); // true는 부모와 별개로 카메라 방향으로 바라보도록 하는 것
    }

    public void OnDrag(PointerEventData eventData)
    {   // 마우스의 위치값에 따라 아이템의 포지션을 바꾼다
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 다시 초기화한다.
        transform.position = startPosition; // 위치를 시작 지점으로
        GetComponent<CanvasGroup>().blocksRaycasts = true; // 레이케스트 차단
        transform.SetParent(originalParent, true); // 원래부모로 복원

        IDragDestination<T> container;
        
        // UI위에 위에 있는 것이 아닌 경우, 따라서 아이템을 버린경우
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
            // 아이템을 넘겨주는 코드
            DropItemIntoContainer(container);
        }
    }

    /// <summary>
    /// 컨테이너를 가져오는 메소드
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


    // 컨테이너로 아이템을 드롭하는 메소드
    private void DropItemIntoContainer(IDragDestination<T> destination)
    {
        Debug.Log("DropItemSeccese");
        // 같은 곳에서 생성되었는지를 체크하는 구문
        if (object.ReferenceEquals(destination, source)) return;

        var destinationContainer = destination as IDragContainer<T>;
        var sourceContainer = source as IDragContainer<T>;

        // 아이템을 교환할 수 없는 경우 => 상대 슬롯이 비어있는 경우
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
        
        // 아이템 숫자 비교해서 넣는거

        destination.AddItems(draggingItem, draggingNumber);
    }

    
}
