using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour ,IDropHandler {
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount ==0)
		{
			Drag.draggingItem.transform.SetParent(this.transform);
			Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;

			GameManager.instance.AddItem(item);

		}
    }
}
