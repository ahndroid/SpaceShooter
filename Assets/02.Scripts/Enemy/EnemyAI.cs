using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {


	public enum State {

		PATROL,
		TRACE,
		ATTACK,
		DIE

	}

	public State state = State.PATROL;

	private Transform playerTr; //주인공 위치 

	private Transform enemyTr; //적 캐릭터의 위치

	private Animator animator;
	private readonly int hashMove = Animator.StringToHash("IsMove"); //파라미터의 해시값을 캐시
	private readonly int hashSpeed = Animator.StringToHash("Speed"); //파라미터의 해시값을 캐시

	private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
	private readonly int hashWalkCycleOffset = Animator.StringToHash("Offset");

	private readonly int hashDie = Animator.StringToHash("Die");
	private readonly int hashDieIdx = Animator.StringToHash("DieIdx");

	private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
	public float attackDistance = 5.0f; //사정거리
	public float traceDistance = 10.0f ; // 추적 사정거리

	public bool isDie = false; //사망여부 판단

	private WaitForSeconds ws ; //코루틴에서 ㅅ용할 지연시간 변수

	private MoveAgent moveAgent; //move agent 스트립트 컴포넌트 제어를 위해

	private EnemyFire enemyFire; //총쏘기 액션
	private void Awake() {
		var player = GameObject.FindGameObjectWithTag("PLAYER");

		if (player != null)
		{
			playerTr = player.GetComponent<Transform>();
		}

		enemyTr = GetComponent<Transform>();
		ws = new WaitForSeconds(0.3f); //코루틴 지연시간 설정

		//더 나은 방법을 사용하기 위해
		attackDistance = attackDistance * attackDistance;
		traceDistance = traceDistance * traceDistance;

		moveAgent = GetComponent<MoveAgent>();

		animator = GetComponent<Animator>();

		enemyFire = GetComponent<EnemyFire>();

		animator.SetFloat(hashWalkCycleOffset,UnityEngine.Random.Range(0.0f,1.0f));
		animator.SetFloat(hashWalkSpeed,UnityEngine.Random.Range(1.0f,1.2f));

	}

	private void OnEnable() {
		//코루틴 시작
		StartCoroutine(CheckState()); //플레이어 위치와 적 위치를 입력으로 상태를 판단한다.
		StartCoroutine(Action()); // 상태에 따른 액션을 취한다.

		Damage.OnPlayerDie +=this.OnPlayerDie; 
	}

    private IEnumerator Action()
    {
        while(!isDie)
		{
			yield return ws;

			switch(state)
			{
				case State.PATROL:
					enemyFire.isFire = false;
					moveAgent.Patrolling = true;
					animator.SetBool(hashMove,true);
					break;
				case State.ATTACK:
					moveAgent.Stop();
					animator.SetBool(hashMove,false);

					if (!enemyFire.isFire)
					{
						enemyFire.isFire = true;
					}
					break;
				case State.TRACE:
					enemyFire.isFire = false;
					moveAgent.TraceTarget = playerTr.position;
					animator.SetBool(hashMove,true);
					
					break;

				case State.DIE:

					this.gameObject.tag ="Untagged";// 죽은경우 태그를 변경해서 적 숫자에서 제외시킨다.
					isDie = true;
					enemyFire.isFire = false; //사격 중지

					moveAgent.Stop(); //순찰 및 추적 중지

					//사망 애니 랜덤 선택
					animator.SetInteger(hashDieIdx,UnityEngine.Random.Range(0,3));
					animator.SetTrigger(hashDie);
					//캡슐 콜라이더 비활성화
					GetComponent<CapsuleCollider>().enabled =false;

					break;
			}
		}
    }

    private IEnumerator CheckState()
    {
        while(!isDie)
		{
			if (state == State.DIE) yield break; //죽었으면 코루틴 중지;

			//float dist = Vector3.Distance(playerTr.position, enemyTr.position); //캐릭간 거리 측정
			//더 나은 방법
			float dist = (playerTr.position - enemyTr.position).sqrMagnitude;

			//공격 사정거리 이내라면
			if (dist <= attackDistance)
			{
				state = State.ATTACK;
			}
			else if (dist <= traceDistance)
			{
				state = State.TRACE;
			}
			else
			{
				state = State.PATROL;
			}

			yield return ws;
		}
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetFloat(hashSpeed,moveAgent.speed);
	}

	public void OnPlayerDie(){

		moveAgent.Stop();
		enemyFire.isFire = false;

		StopAllCoroutines(); //모든 코루틴 중지

		animator.SetTrigger(hashPlayerDie);
	}
}
