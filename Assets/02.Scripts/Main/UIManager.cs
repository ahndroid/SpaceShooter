using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public void OnClickStartBtn(){

		Debug.Log("Clicked Start Button");

		//SceneManager.LoadScene("Level1");
		//SceneManager.LoadScene("Play",LoadSceneMode.Additive);

		SceneManager.LoadScene("SceneLoader");
	}


	public void OnClickOptionBtn(){

		Debug.Log("Clicked Option");
	}

	public void OnClickCreditBtn(){

		Debug.Log("Clicked Credit");
	}
}
