using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static PlayerData data = new PlayerData();
	const string PlayerData = "PlayerData";

	public static void Save ()
	{

#if UNITY_WEBGL
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(PlayerData, json);
        PlayerPrefs.Save(); // Ensure data is saved
#else
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		bf.Serialize (file, SaveLoad.data);
		file.Close ();
#endif
    }

	public static void Load ()
	{
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(PlayerData))
        {
            string json = PlayerPrefs.GetString(PlayerData);
            data = JsonUtility.FromJson<PlayerData>(json);
        }
#else
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			SaveLoad.data = (PlayerData)bf.Deserialize (file);
			file.Close ();
		}
#endif
    }

}
