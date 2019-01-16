using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {


	float h= 0.0f;
	float v= 0.0f;

	float r = 0.0f;

	public float moveSpeed = 10.0f;

	public float rotSpeed = 80.0f;

	Transform tr;
	// Use this for initialization

	
	public PlayerAnimation playerAnim;

	public Animation anim;


	private void OnEnable() {
		GameManager.OnItemChange +=UpdateSetup;
	}

    private void UpdateSetup()
    {
        moveSpeed = GameManager.instance.gameData.speed;
    }

    void Start () {
		
		tr = GetComponent<Transform>();
		anim = GetComponent<Animation>();
		anim.clip = playerAnim.idle;
		anim.Play();

		moveSpeed = GameManager.instance.gameData.speed;
	}
	
	// Update is called once per frame
	void Update () {
		
		h= Input.GetAxis("Horizontal");
		v= Input.GetAxis("Vertical");
		r = Input.GetAxis("Mouse X");

		//위치 이동
		Vector3 moveVector = Vector3.forward * v + Vector3.right * h;

		tr.Translate(moveVector.normalized * moveSpeed * Time.deltaTime, Space.Self);
		tr.Rotate(  Vector3.up * rotSpeed * Time.deltaTime * r);

		// 애니메이션 변경
		string clipName= string.Empty;

		if ( v >= 0.1f)
		{
			clipName = playerAnim.runF.name;
		}
		else if (v <= -0.1f)
		{
			clipName = playerAnim.runB.name;
		}
		else if ( h >= 0.1f )
		{
			clipName = playerAnim.runR.name;
		}
		else if ( h <= -0.1f )
		{
			clipName = playerAnim.runL.name;
		}
		else
		{
			clipName = playerAnim.idle.name;
		}

		anim.CrossFade(clipName, 0.3f);
	}


	
}

[System.Serializable]
public class PlayerAnimation
{
	public AnimationClip idle;
	public AnimationClip runF;

	public AnimationClip runB;

	public AnimationClip runL;

	public AnimationClip runR;

}