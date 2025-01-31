using UnityEngine;

public class CircleMove : MonoBehaviour
{
    //Circle을 지정된 위치로 Lerp 시키는 스크립트
    public GameObject Circle;
    Vector3 pos = new Vector3(5, -3, 0);





    // Update is called once per frame
    void Update()
    {
        //Circle.transform.position = Vector3.Lerp(Circle.transform.position, pos, Time.deltaTime);
        //0을 입력할 경우 움직이지 않습니다.
        //1이 최대치입니다.

        //일정한 속도로 목표 지점까지 이동하게 만드는 스크립트
        //Vector3.MoveTowards(시작 지점, 끝 지점, 이동속도);
        //Circle.transform.position = Vector3.MoveTowards(Circle.transform.position, pos, Time.deltaTime);

        Circle.transform.position = Vector3.Slerp(Circle.transform.position, pos, 0.05f);
        //Lerp랑 비슷하지만 선형이 아닌 호를 그리면서 이동(시작 지점, 끝 지점, 보정값);


    }
}
