using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform player;   

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    //카메라가 제일 늦게 움직여야 함 다 같은 업데이트문이면 어느게 먼저 될지 모름
    void LateUpdate()
    {
        transform.position = player.position;

    }
}
