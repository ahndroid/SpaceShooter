using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour ,IDragHandler,IBeginDragHandler,IEndDragHandler {



	private Transform itemTr;

	private Transform inventoryTr;

	private Transform itemListTr;

	private CanvasGroup canvasGroup;

	public static GameObject draggingItem = null; //현재 드레깅 되고 있는 아이템 참조



    // Use this for initialization
    void Start () {
		itemTr = GetComponent<Transform>();
		inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();

		itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();

		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void OnDrag(PointerEventData eventData)
    {
        //드레그가 발생하면 아이템의 위치를 마우스 포인터에 맞춘다.
		itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventoryTr); //gird layout group 컴포넌트에서 벗어나 상위에 인벤토리 객체의 자식 객체로 바꾼다. 영향을 받지 않도록.
		draggingItem = this.gameObject;

		canvasGroup.blocksRaycasts = false; // 드레그 시작되면 다른 UI 이벤트를 받지 않도록 한다. 그래야 Drop 이벤트가 타켓쪽에서 제대로 발생한다.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem =null;

		//드레그 종료되면 다른 UI 이벤트를 받을 수 있게 설정한다.
		canvasGroup.blocksRaycasts = true;

		//슬롯이 아닌경우 다시 원래대로 돌린다.
		if (itemTr.parent == inventoryTr)
		{
			itemTr.SetParent(itemListTr.transform);
		}
    }


	/*
	Drop 구현은 드롭되는 객체에 스크립트로 구현한다.
	 */

    // public void OnDrop(PointerEventData eventData)
    // {
    //     throw new System.NotImplementedException();
    // }
}
