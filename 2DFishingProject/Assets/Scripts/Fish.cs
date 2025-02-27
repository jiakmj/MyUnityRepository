using System.IO;
using System;
using UnityEngine;

[Serializable]
public class Fish
{
    public string name;
    public int weight; //����ġ
    public int level; //���̵�
    public Sprite sprite; //���� �ε� �� ����
    public string spritePath; //JSON���� ��� ����

    public void LoadSprite()
    {
        sprite = Resources.Load<Sprite>(spritePath);
    }
}
