using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform player;   

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    //ī�޶� ���� �ʰ� �������� �� �� ���� ������Ʈ���̸� ����� ���� ���� ��
    void LateUpdate()
    {
        transform.position = player.position;

    }
}
