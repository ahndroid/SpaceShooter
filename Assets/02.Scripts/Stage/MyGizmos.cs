using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
눈에 안보이는 게임오브젝트를 표시하기 위한 것.
 */
public class MyGizmos : MonoBehaviour {


	public enum Type { Normal, WayPoint}

	private const string wayPointFile ="Enemy";

	public Type type = Type.Normal;

	public Color color = Color.yellow;

	public float radius = 0.3f;
	//se this for initialization
	private void OnDrawGizmos() {


		if (type == Type.Normal)
		{
			Gizmos.color = color;
			Gizmos.DrawSphere(transform.position,radius);
		}
		else
		{
			Gizmos.color = color;
			Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, wayPointFile,true);
			Gizmos.DrawWireSphere(transform.position,radius);
		}
		
	}
}
