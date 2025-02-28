using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[Serializable]
public class FishData
{
    public string name;
    public int weight; //가중치
    public int level; //난이도
    public string spritePath; //JSON에서 경로 저장
}

[Serializable]
public class FishDatabase
{
    public List<FishData> fishList;
}

public class FishManager : MonoBehaviour
{
    public List<Fish> fishes = new List<Fish>(); 

    void Start()
    {
        LoadFishData();
    }

    void LoadFishData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "fishData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            FishDatabase fishDatabase = JsonUtility.FromJson<FishDatabase>(json);

            foreach (var fishData in fishDatabase.fishList)
            {
                Fish fish = new Fish
                {
                    name = fishData.name,
                    weight = fishData.weight,
                    level = fishData.level,
                    spritePath = fishData.spritePath,
                };
                fish.LoadSprite();

                fishes.Add(fish);
            }
        }
    }

    public Fish GetRandomFish()
    {
        if (fishes.Count == 0)
        {
            return null;
        }
        return fishes[UnityEngine.Random.Range(0, fishes.Count)];

        //int totalWeight = 0;

        //foreach (var fish in fishList)
        //{
        //    totalWeight += fish.weight;
        //}

        //int randomValue = UnityEngine.Random.Range(0, totalWeight);        

        //int currentWeight = 0;

        //foreach (var fish in fishList) 
        //{
        //    currentWeight += fish.weight;
        //    if (randomValue < currentWeight)
        //    {
        //        return fish;
        //    }
        //}
        //return null;
    }
}
