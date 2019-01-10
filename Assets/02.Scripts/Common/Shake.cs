using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	// Use this for initialization

	public Transform shakeCamera;

	private Vector3 originPos; //shake 전 위치 저장
	private Quaternion originRot ; //shake 전 회전값 저장

	public bool shakeRotate = false;

	void Start () {
		
		//카메라의 초기 값 저장
		originPos = shakeCamera.localPosition;
		originRot = shakeCamera.localRotation;
	}
	
	public IEnumerator ShakeCamera(float duration = 0.05f, float maginitudePos = 0.03f, float magnitudeRot = 0.1f)
	{

		//지난 시간을 누적
		float passTime = 0.0f;

		while(passTime < duration)
		{
			//불규칙한 위치 산출
			Vector3 shakePos = Random.insideUnitSphere;
			shakeCamera.localPosition = shakePos * maginitudePos;

			if(shakeRotate)
			{
				Vector3 shakeRot = new Vector3(0,0,Mathf.PerlinNoise(Time.time * magnitudeRot,0.0f));

				shakeCamera.localRotation = Quaternion.Euler( shakeRot);
			}

			//진동 시간을 누적
			passTime += Time.deltaTime;

			yield return null;
		}

		shakeCamera.localPosition = originPos;
		shakeCamera.localRotation = originRot;
		
	}
}
