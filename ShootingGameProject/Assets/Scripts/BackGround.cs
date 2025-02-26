using UnityEngine;

//배경 스크롤 기능을 만들겁니다.
//확인한 것: 마테리얼의 offset을 건드렸더니 이미지가 밀린다.
//필요한 것: 마테리얼 정보, 스크롤링 속도
//어떻게 진행 할 것인가? 위로? 아래로? 옆으로?

public class BackGround : MonoBehaviour
{
    public Material backgroundMaterial;
    public float scrollSpeed = 0.2f;
   

    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = Vector2.up;

        backgroundMaterial.mainTextureOffset += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}
