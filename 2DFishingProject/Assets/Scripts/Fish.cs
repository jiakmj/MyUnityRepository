using System.IO;
using System;
using UnityEngine;

[Serializable]
public class Fish
{
    public string name;
    public int weight; //가중치
    public int level; //난이도
    public Sprite sprite; //사진 로드 후 저장
    public string spritePath; //JSON에서 경로 저장

    public void LoadSprite()
    {
        sprite = Resources.Load<Sprite>(spritePath);
    }
}
