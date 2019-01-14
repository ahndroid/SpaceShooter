using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

	// Use this for initialization


	[Header("Enemy Create Info")]
	public Transform [] points; //스폰 장소

	public GameObject enemy;

	public float createTime = 2.0f; //스폰주기

	public int maxEnemy =10;

	public bool isGameOver = false;

	public static GameManager instance = null;

	[Header("Object Pool")]
	public GameObject bulletPrefab;

	public int maxPool = 10;

	public List<GameObject> bulletPool = new List<GameObject>();

	private void Awake() {
		if (instance == null)
		{
			instance = this;
		}
		else if (instance !=this)
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject); //다른 씬으로 넘어가더라도 겜오브젝트를 삭제하지 않음


		CreatePooling();

	}

    private void CreatePooling()
    {
        //페어런트 객체 생성
		GameObject objPools = new GameObject("ObjectPools");

		for (int i =0 ; i < maxPool ; i++)
		{
			var bullet = Instantiate<GameObject>(bulletPrefab, objPools.transform);
			bullet.name ="Bullet_"+i.ToString("00");

			bullet.SetActive(false); //비활성화

			bulletPool.Add(bullet);

		}
    }

    void Start () {
		
		points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

		if (points.Length > 0)
		{
			StartCoroutine(CreateEnemy());
		}
	}

    private IEnumerator CreateEnemy()
    {
        while(!isGameOver)
		{
			int enemyCount = GameObject.FindGameObjectsWithTag("ENEMY").Length;

			if (enemyCount < maxEnemy)
			{
				yield return new WaitForSeconds(createTime); //생성주기 만큼 대기

				int idx = UnityEngine.Random.Range(1,points.Length);

				Instantiate(enemy , points[idx].position,points[idx].rotation);
			}
			else
			{
				yield return null;
			}
		}
    }

    public GameObject GetBullet(){

		//var bullet = bulletPool.First(b=>b != null && b.activeSelf ==false); //비활성화 여부로 사용가능 여부를 판단

		for (int i = 0; i < bulletPool.Count ; i++)
		{
			if (bulletPool[i].activeSelf == false)
			{
				return bulletPool[i];
			}
		}

		return null;

		//return bullet;
	}


	private bool isPaused = false;

	public void OnPauseClick(Text senderText)
	{
		isPaused = !isPaused;

		if (isPaused) 
			senderText.text = ">>";
		else
		{
			senderText.text = "||";
		}

		Time.timeScale = (isPaused) ? 0.0f:1.0f; // timeScale 이 0 이면 정지, 1이면 정상속도

		//유저 액션에 반응하는 스크립트를 모두 찾아 비활성/활성화 처리
		var playerObj = GameObject.FindGameObjectWithTag("PLAYER");

		var scripts = playerObj.GetComponents<MonoBehaviour>();

		foreach(var script in scripts)
		{
			script.enabled = !isPaused;
		}

		//정지되면 무시 선택버튼을 비활성하기
		var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = !isPaused;


	}
}
