using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour {




	private Camera uiCamera;

	private Canvas canvas;

	private RectTransform rectParent;

	private RectTransform rectHp;

	[HideInInspector]
	public Vector3 offset = Vector3.zero; //위치 조절 오프셋
	[HideInInspector]	
	public Transform targetTr; //대상의 Tr

	// Use this for initialization
	void Start () {
		
		canvas = GetComponent<Canvas>();
		uiCamera = canvas.worldCamera;
		rectParent = canvas.GetComponent<RectTransform>();
		rectHp = this.gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LateUpdate() {

		//월드 좌표 => 스크린자표 => 로컬좌표 처리해 hp 바 위치를 잡는다.
		var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset); //스크린좌표는 2d 이며 z 는 월드좌표값이 넘어온다.

		if (screenPos.z < 0.0f)
		{
			screenPos *= -1.0f; //카메라 뒷쪽(180도 회전한 쪽)인 경우 보정,즉 따라다니는 카메라의 z 값이 음수라도 캐릭터는 보이지 않는데 hp가 표시되므로 이를 보정해 안보이게 한다.
		}

		var localPos = Vector2.zero;

		//스크린 좌표를 다시 캔버스 로컬 좌표계로 변형한다.
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent,screenPos,uiCamera,out localPos);

		rectHp.localPosition = localPos;
	}
}
