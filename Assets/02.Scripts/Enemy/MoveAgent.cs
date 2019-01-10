using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour {



	private readonly float patrolSpeed = 1.5f;
	private readonly float traceSpeed = 4.0f;

	private float damping = 1.0f; //회전 감쇄 계수

	private Transform enemyTr;

	private bool patrolling;

	public bool Patrolling { 
		get {	return patrolling; }

		set {
			patrolling = value;

			if (patrolling)
			{
				agent.speed = patrolSpeed;
				damping =1.0f; //패트롤시 회전계수
				MoveWayPoint();
			}
		 }
	 }

	//Animator 에서 참조 할 속도 값
	public float speed {

		get{ return agent.velocity.magnitude ;}
	}

	//private Animator animator;

	

	private Vector3 traceTarget; //추적대상 위치

	public Vector3 TraceTarget { 
		get { return traceTarget;}
		set {
			traceTarget = value;
			agent.speed = traceSpeed;
			damping = 7.0f;
			DoTraceTarget(traceTarget);
		}
	 }

    private void DoTraceTarget(Vector3 target)
    {
        if (agent.isPathStale) return;

		agent.destination = target;
		agent.isStopped = false;
    }


	public void Stop(){

		agent.isStopped = true;
		agent.velocity = Vector3.zero;
		patrolling = false;

	}

    // Use this for initialization
    public List<Transform> wayPoints;

	public NavMeshAgent agent;

	//다음 순찰 포인트
	public int nextIdx =0;

	void Start () {


		//회전 조정용
		enemyTr = GetComponent<Transform>();
	

		agent = GetComponent<NavMeshAgent>();
		agent.autoBraking = false; //목적징 가까워 질 때 속도 줄이기 끔
		agent.updateRotation = false ; // 자동 회전 비활성화 수동으로 조절할 것이므로..

		var group = GameObject.Find("WayPointGroup");

		if (group != null)
		{
			// s 가 붙었는지 확인해야 한다. getcomponents <==
			// group 객체 및 하위 객체 전체의 transform 을 가져와 리스트에 담아준다.
			group.GetComponentsInChildren<Transform>(wayPoints);
			//첫번째 삭제. 페어런트인 group 객체도 Transform 컴포넌트를 갖는 경우 이 것이 첫번째에 포함된다.
			wayPoints.RemoveAt(0); 

			//첫 번째 이동 장소를 불규칙하게..
			nextIdx = UnityEngine.Random.Range(0,wayPoints.Count);
		}

		this.Patrolling = true;
		//MoveWayPoint();

		
	}

    private void MoveWayPoint()
    {
        if(agent.isPathStale) return ; //아직 경로 계산이 끝나지 않음.

		agent.destination = wayPoints[nextIdx].position;
		agent.isStopped = false ; // 이동시작
    }

    // Update is called once per frame
    void Update () {
		
		//캐릭터가 이동 중일 때만 회전을 하므로 여기서 회전을 보정한다.
		if (agent.isStopped == false)
		{
			//NavMeshAgent 가 가야 할 방향 벡터를 쿼터니언 타입의 각도로 변환
			Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
			//보간 함수를 이용해 점진적으로 회전시킴
			//damping 값이 클수록 빨리 회전함.
			enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation,rot,Time.deltaTime * damping);
		}
	
		//이동중이고, 목적지가 얼마남지 않았다면
		//agent.velocity.magnitude 를 사용하는 것보다 제곱근 연산을 피하기 위해 sqrMagnitude 를 이용한다.
		if (patrolling && agent.velocity.sqrMagnitude >=0.2f * 0.2f && agent.remainingDistance <=0.5f)
		{
			//nextIdx = ++nextIdx % wayPoints.Count; //다음 목적지 설정

			//첫 번째 이동 장소를 불규칙하게..
			nextIdx = UnityEngine.Random.Range(0,wayPoints.Count);
			
			MoveWayPoint(); //다음 목적지로..
		}
	}
}
