using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataInfo;

using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {

	private string dataPath;

	public void Initialize(){

		dataPath = Application.persistentDataPath + "/gameData.dat";

	}


	public void Save(GameData gameData)
	{
		BinaryFormatter bf = new BinaryFormatter();

		FileStream fs = File.Create(dataPath);

		GameData data = new GameData();

		data.damage = gameData.damage;
		data.equipItem = gameData.equipItem;
		data.hp = gameData.hp;
		data.killCount = gameData.killCount;
		data.speed = gameData.speed;

		bf.Serialize(fs, data); //파일에 데이터 기록.
		fs.Close();
	}



	public GameData Load()
	{
		GameData ret = null;

		if (File.Exists(dataPath))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Open(dataPath,FileMode.Open);

			ret= bf.Deserialize(fs) as GameData;

		}
		else
		{
			//파일이 없으면 기본값
			ret = new GameData();
		}

		return ret;
	}
}
