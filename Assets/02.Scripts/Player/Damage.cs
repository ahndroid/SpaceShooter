using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour {

	// Use this for initialization

	private const string bulletTag ="BULLET";

	private float initHp = 100.0f;

	public float currHp;

	public delegate void PlayerDieHandler();

	public static event PlayerDieHandler OnPlayerDie; //이벤트 선언

	public Image bloodScreen ; //혈흔 이미지

	public Image hpBar;

	private readonly Color initColor = new Vector4(0,1.0f,0.0f,1.0f); //초기 생명바 색, 녹색

	private Color currHpBarColor; //현재 생명바 색상

	private void OnEnable() {
		GameManager.OnItemChange +=UpdateSetup;
	}

    private void UpdateSetup()
    {
        initHp = GameManager.instance.gameData.hp;
		currHp += GameManager.instance.gameData.hp - currHp;
    }

    void Start () {
		
		initHp = GameManager.instance.gameData.hp;
		currHp = initHp;

		hpBar.color = initColor;
		currHpBarColor = initColor;
	}
	
	private void OnTriggerEnter(Collider coll) {
		
		if (coll.tag == bulletTag)
		{
			Destroy(coll.gameObject);

			StartCoroutine(ShowBloodScreen()); //혈흔 나타났다 사라지기

			currHp -= 5.0f;

			Debug.Log("Player HP = "+currHp.ToString());

			DisplayHpBar();

			if (currHp <=0.0f)
			{
				PlayerDie();
			}
		}
	}

    private void DisplayHpBar()
    {
		var factor = (currHp / initHp);

        if ((currHp / initHp) > 0.5f) //50% 까지는노란색
		{
			currHpBarColor.r = (1-factor) * 2.0f;
		}
		else
		{
			currHpBarColor.g = factor * 2.0f;
		}

		hpBar.color = currHpBarColor;
		hpBar.fillAmount = factor ;
    }

    private IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1,0,0,UnityEngine.Random.Range(0.2f,0.3f)); //알파값을 불규칙하게..

		yield return new WaitForSeconds(0.1f);

		bloodScreen.color = Color.clear; // 0.1초 보였다가 다시 안보이게...
    }

    private void PlayerDie()
    {
        Debug.Log("Player Die");

		OnPlayerDie();
		GameManager.instance.isGameOver = true; //플레이어가 사망한 경우 게임을 종료 , 억지로 static 사용법을 설명하기 위해서..원래는이벤트에서 처리해야 할듯
    }
}
