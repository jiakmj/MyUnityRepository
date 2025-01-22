using UnityEngine;
using System;

//2. ���Ϲٱ���
[Flags]
public enum BASKET
{
    None = 0,
    ��� = 1 << 0,
    �� = 1 << 1,
    ������ = 1 << 2,
    �� = 1 << 3,
    ü�� = 1 << 4,
    ���� = 1 << 5
}
//5. ������
public enum RAINBOW
{
    ����, ��Ȳ, ���, �ʷ�, �Ķ�, ����, ����
}

//1. Add ComponentMenu ��� �߰��� �����̸�/Ŭ���� �˻��� ����
[AddComponentMenu("MJ/Sample01")]

public class Report : MonoBehaviour
{
    //2. ���Ϲٱ���, 5. ������
    public string[] FruitName;
    public BASKET Fruit;
    public RAINBOW Rainbow;

    //3. ��
    int Money;

    //1. �������ɿ���
    [Tooltip("üũ�Ǹ� ���� ������ �������� �ǹ��մϴ�.")]
    public bool isJump = true;


    //4. �ʵ��
    [Range(1, 99)]
    public int fieldView;

    
}
