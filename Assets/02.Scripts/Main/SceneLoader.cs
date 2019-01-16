using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


	public CanvasGroup fadeCg;

	[Range(0.5f,2.0f)]
	public float fadeDuration = 1.0f; //페이드 인 처리 시간

	public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>(); //호출할 씬을 저장

	// Use this for initialization
	IEnumerator Start () {

		fadeCg.alpha = 1.0f;

		InitSceneInfo();

		foreach(var scene in loadScenes)
		{
			yield return StartCoroutine(LoadScene(scene.Key,scene.Value));
		}
		
		StartCoroutine(Fade(0.0f));
	}

    private IEnumerator Fade(float v)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));

		fadeCg.blocksRaycasts = true;

		float fadeSpeed = Mathf.Abs(fadeCg.alpha - v) / fadeDuration;

		while(!Mathf.Approximately(fadeCg.alpha,v))
		{
			fadeCg.alpha = Mathf.MoveTowards(fadeCg.alpha, v, fadeSpeed * Time.deltaTime); //알파값 보간

			yield return null;
		}

		fadeCg.blocksRaycasts = false;

		SceneManager.UnloadSceneAsync("SceneLoader");
    }

    private IEnumerator LoadScene(string key, LoadSceneMode value)
    {
        yield return SceneManager.LoadSceneAsync(key, value);//비동기 방식으로 씬을 로드 후 대기

		Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount -1);
		SceneManager.SetActiveScene(loadedScene);


    }

    // Update is called once per frame
    void Update () {
		
	}


	private void InitSceneInfo()
	{
		loadScenes.Add("Level1", LoadSceneMode.Additive);
		loadScenes.Add("Play", LoadSceneMode.Additive);
	}
}
