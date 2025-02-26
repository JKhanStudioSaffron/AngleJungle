using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose: Manages trophy instantiation and shuffling based on level progression.
/// Responsibilities:
/// - Ensures a singleton instance for global trophy management.
/// - Shuffles trophy rewards based on saved data order.
/// - Instantiates trophies at designated positions for unlocked levels.
/// - Provides a method to instantiate a specific level's trophy at a given location.
/// Usage:
/// - Attach to a persistent GameObject in the scene.
/// - Assign LevelRewardData list with trophy rewards.
/// - Call CreateTrophy() to instantiate trophies up to the player's current level.
/// - Call CreateTrophyOfLevel() to get a specific level's trophy instance.
/// </summary>

public class Trophys : MonoBehaviour {

    public static Trophys Instance;

    public List<GameObject> TrophyList { get; set; }
    public List<LevelRewardData> Data;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ShuffleTrophies();
    }

    public void ShuffleTrophies()
    {
        List<GameObject> trophies = new();
        for (int i = 0; i < SaveLoad.data.TrophyOrder.Count; i++)
        {
            trophies.Add(Data[SaveLoad.data.TrophyOrder[i] - 1].Reward);
            Data[SaveLoad.data.TrophyOrder[i] - 1].Reward = null;
        }

        for (int i = 0; i < trophies.Count; i++)
        {
            Data[i].Reward = trophies[i];
        }
    }

    public void CreateTrophy(int levelProgress, List<Transform> trophyPlace)
    {
        int index = 0;
        TrophyList = new();
        foreach (var item in Data)
        {
            if (levelProgress > item.LevelNoToOpenAt)
            {
                TrophyList.Add(Instantiate(item.Reward, trophyPlace[index].position, trophyPlace[index].rotation));
                index++;
            }
        }
    }

    public GameObject CreateTrophyOfLevel(int currentLevel, Transform place)
    {
        GameObject reward = Data.Find(rwd => rwd.LevelNoToOpenAt == currentLevel).Reward;
        return Instantiate(reward, place.position, place.rotation);
    }
}


