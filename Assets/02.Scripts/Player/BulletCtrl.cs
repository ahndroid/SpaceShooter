using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

	// Use this for initialization

	public float damage = 20.0f;
	public float speed = 4000.0f;


	// 풀에서 꺼내 활성화 될때 총알이 동작되도록 수정해야 하므로 주석처리한다.
	// void Start () {
		
	// 	//힘을 줄때 이 객체의 z 방향으로 힘을 줘야 한다. 월드 좌표가 아니라..
	// 	GetComponent<Rigidbody>().AddForce(transform.forward * speed);

	// 	//만약 월드좌표 기준으로 힘을 주려면
	// 	//GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
	// }
	

	//사용된 총알을 다시 초기화 해서 재사용 해야 하므로 아래 컴포넌트를 캐시한다.
	private Transform tr;

	private Rigidbody rb;

	private TrailRenderer trail;

	private void Awake() {
		
		tr = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
		trail = GetComponent<TrailRenderer>();
	}

	private void OnEnable() {
		rb.AddForce(transform.forward * speed);
	}


	private void OnDisable() {
		
		trail.Clear();
		tr.position = Vector3.zero;
		tr.rotation = Quaternion.identity;
		rb.Sleep();
	}

	private void OnDestroy() {
		//Debug.Log("Destroyed");
	}

}
