using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour {

	// Use this for initialization

	private float hp = 100.0f;

	private const string bulletTag ="BULLET";

	private GameObject bloodEffect;


	private float initHp = 100.0f;
 
 
	public GameObject hpBarPrefab; //생명 게이지 프리팹

	public Vector3 hpBarOffset = new Vector3(0,2.2f,0); //생명게이지 오프셋

	private Canvas uiCanvas ; //생명게이지가 표시될 캔버스

	private Image hpBarImage; //남은 생명을 표시할 이미지

	void Start () {
		
		bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect"); // resources 폴더를 루트로 하는 경로에서 찾음.

		SetHpBar();
	}

    private void SetHpBar()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update () {
		
	}

	
	private void OnCollisionEnter(Collision coll) {
		if (coll.collider.tag == bulletTag)
		{
			ShowBloodEffect(coll); //혈흔 효과 보이기
			hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
			
			//Destroy(coll.gameObject);//총알 삭제

			coll.gameObject.SetActive(false); //총알 풀로 반환

			

			if (hp <=0.0f)
			{
				GetComponent<EnemyAI>().state=EnemyAI.State.DIE;
			}
		}
	}

    private void ShowBloodEffect(Collision coll)
    {
        Vector3 pos = coll.contacts[0].point; //충돌점

		Vector3 normal = coll.contacts[0].normal; //충돌점의 법선 벡터

		Quaternion rot = Quaternion.FromToRotation(-Vector3.forward , normal); //뒷쪽을 향해 그려져야 하므로..

		//혈흔 효과 생성
		GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);

		Destroy(blood,1.0f);
    }
}
