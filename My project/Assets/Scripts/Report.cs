using UnityEngine;
using System;

//2. 과일바구니
[Flags]
public enum BASKET
{
    None = 0,
    사과 = 1 << 0,
    귤 = 1 << 1,
    복숭아 = 1 << 2,
    배 = 1 << 3,
    체리 = 1 << 4,
    딸기 = 1 << 5
}
//5. 무지개
public enum RAINBOW
{
    빨강, 주황, 노랑, 초록, 파랑, 남색, 보라
}

//1. Add ComponentMenu 기능 추가로 본인이름/클래스 검색이 가능
[AddComponentMenu("MJ/Sample01")]

public class Report : MonoBehaviour
{
    //2. 과일바구니, 5. 무지개
    public string[] FruitName;
    public BASKET Fruit;
    public RAINBOW Rainbow;

    //3. 돈
    int Money;

    //1. 점프가능여부
    [Tooltip("체크되면 점프 가능한 상태임을 의미합니다.")]
    public bool isJump = true;


    //4. 필드뷰
    [Range(1, 99)]
    public int fieldView;

    
}
