using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DataInfo;

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

	public CanvasGroup InventoryCanvasGroup;

	// [HideInInspector]
	// public int killCount;
	[Header("GameData")]


	public Text KillCountText; //킬카운트 보여줄 텍스트

	private DataManager dataManager;

	public GameData gameData;

	private void Awake() {
		if (instance == null)
		{
			instance = this;
		}
		else if (instance !=this)
		{
			Destroy(this.gameObject);
		}

		dataManager = GetComponent<DataManager>();
		dataManager.Initialize();

		DontDestroyOnLoad(this.gameObject); //다른 씬으로 넘어가더라도 겜오브젝트를 삭제하지 않음


		CreatePooling();

		LoadGameData();

	}

    private void LoadGameData()
    {
        // killCount = PlayerPrefs.GetInt("KILL_COUNT",0); //해당키가 없으면 기본값 0 리턴
		// KillCountText.text ="Kill : "+ killCount.ToString("0000");

		GameData data = dataManager.Load();

		gameData = data;

		KillCountText.text ="Kill : "+ gameData.killCount.ToString("0000");
    }

	public void IncreaseKillCount()
	{
		++gameData.killCount;
		KillCountText.text = "Kill : "+gameData.killCount.ToString("0000");

		//PlayerPrefs.SetInt("KILL_COUNT",killCount);
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

		OnInventoryOpen(false);
		
		points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

		if (points.Length > 0)
		{
			StartCoroutine(CreateEnemy());
		}
	}

    public void OnInventoryOpen(bool isOpened)
    {
        InventoryCanvasGroup.alpha = (isOpened) ? 1.0f:0.0f;
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

	void SaveGameData()
	{
		dataManager.Save(gameData);
	}

	private void OnApplicationQuit() {
		SaveGameData();
	}
}
