using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

	// Use this for initialization

	private AudioSource fireAudio;

	private Animator animator;

	private Transform playerTr;

	private Transform enemyTr;


	private readonly int hashFire = Animator.StringToHash("Fire");
	private readonly int hashReload = Animator.StringToHash("Reload");

	private float nextFire = 0.0f; //사격 간격 조절을 위해 다음 사격시간 지정용

	private readonly float fireRate = 0.1f;

	private float damping = 10.0f; //주인공을 향해 돌 때 회전계수

	private readonly float reloadTime = 2.0f; //재장전 시간
	private readonly int maxBullet = 10;

	private int currentBullet = 10;

	private bool isReload = false;

	private WaitForSeconds wsReload;

	public bool isFire ;// 현재 사격중인지 여부

	public AudioClip fireSfx ; //사격소리
    public AudioClip reloadSfx;

	public GameObject Bullet;

	public Transform FirePos;

	public MeshRenderer MuzzleFlash;

    void Start () {
		
		fireAudio = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		
		playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
		enemyTr = GetComponent<Transform>();

		wsReload = new WaitForSeconds(reloadTime);

		MuzzleFlash.enabled =false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isReload && isFire)
		{
			if (Time.time >= nextFire)
			{
				Fire();

				//다음 발사 시간을 지정한다
				// Time.time 을 또 호출하면 정확하진 않지만...패스
				// 랜덤값을 추가 해 불규칙하게 총을 쏘도록 한다.
				nextFire = Time.time + fireRate + UnityEngine.Random.Range(0.0f,0.3f);
			}

			//플레이어 쪽으로 점진적으로 회전시킴 - 이건 총을 쏘건 안쏘건 돌아야 함.
			// 플레이어 우치를 향한 회전 각도 구하기
			Quaternion rot = Quaternion.LookRotation(playerTr.position- enemyTr.position);
			// 적의 회전을 rot 만큼 damping 속도로 자연스럽게 회전시킨다.
			enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
		}
	}

    private void Fire()
    {
        animator.SetTrigger(hashFire);
		fireAudio.PlayOneShot(fireSfx,1.0f);

		StartCoroutine(ShowMuzzleFlash());

		//총알 생성
		var bullet = Instantiate(Bullet, FirePos.position, FirePos.rotation);

		//일정 시간 지난 후 삭제
		Destroy(bullet,3.0f);
		

		//재장전 여부 판단
		isReload = (--currentBullet % maxBullet ==0);

		if (isReload)
		{
			StartCoroutine(Reloading());
		}
    }

    private IEnumerator ShowMuzzleFlash()
    {
        MuzzleFlash.enabled = true;

		//불규칙한 각도를 구한다.
		Quaternion rot = Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0,360));

		//z 방향으로 회전시킨다.
		MuzzleFlash.transform.localRotation = rot;
		MuzzleFlash.transform.localScale = Vector3.one * UnityEngine.Random.Range(1.0f, 2.0f);

		//텍스처 offset 을 랜덤하게
		Vector2 offset = new Vector2(UnityEngine.Random.Range(0,2), UnityEngine.Random.Range(0,2)) * 0.5f;

		MuzzleFlash.material.SetTextureOffset("_MainTex", offset);

		//보이는 동안 잠깐 대기
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.2f));

		MuzzleFlash.enabled = false;
    }

    private IEnumerator Reloading()
    {

		MuzzleFlash.enabled = false;
        animator.SetTrigger(hashReload);
		fireAudio.PlayOneShot(reloadSfx,1.0f);

		yield return wsReload; //재장전 애니메이션 대기

		currentBullet = 10;
		isReload =false; 
    }
}
