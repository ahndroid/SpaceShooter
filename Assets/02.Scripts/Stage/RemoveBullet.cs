using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour {

	// Use this for initialization

	public GameObject sparkEffect;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "BULLET")
		{
			ShowEffect(other);
			//Destroy(other.gameObject);
			other.gameObject.SetActive(false);
		}	
	}

    private void ShowEffect(Collision other)
    {
        ContactPoint contact = other.contacts[0];

		//충돌한 상대 오브젝트이 법선 벡터에 마이너스를 해야 현재 오브젝트의 충돌 법선 벡터가 된다.
		Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

		var effect = Instantiate(sparkEffect,contact.point + (-contact.normal * 0.05f),rot);

		effect.transform.SetParent(this.transform);
		
    }
}
