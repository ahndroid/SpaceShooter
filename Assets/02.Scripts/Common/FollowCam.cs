using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	public Transform target;

	public float moveDamping = 15.0f; //이동 속도 개수
	public float rotateDamping = 10.0f; //회전 속도 계수

	public float distance =5.0f; //추적대상과의 거리

	public float height = 4.0f;  //추적 대상과의 높이

	public float targetOffset = 2.0f; //추적 좌표의 오프셋

	private Transform tr;



	void Start () {
		
		tr = GetComponent<Transform>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LateUpdate() {
		var camPos = target.position - (target.forward * distance) + (target.up * height);


		//이동할 때 속도 계수 적용
		tr.position = Vector3.Slerp(tr.position,camPos,Time.deltaTime * moveDamping);
		tr.rotation = Quaternion.Slerp(tr.rotation,target.rotation, Time.deltaTime * rotateDamping);

		//카메라가 좌표를 보면 발바닥쪽을 가르키므로 오프셋 만큼 들어준다.
		//그래야 플레이어가 카메라에 잘 나온다.

		tr.LookAt(target.position + (target.up * targetOffset));
	}

	//추적 할 좌표를 시각적으로 표시
	private void OnDrawGizmos() {
		
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(target.position + (target.up * targetOffset),0.1f); //타겟 표시
		Gizmos.DrawLine(target.position +(target.up * targetOffset), transform.position); //카메라와 타겟 지점 간 선을 표시
	}
}
