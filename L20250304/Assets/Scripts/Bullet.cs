using UnityEngine;

public class Bullet : MonoBehaviour
{    
    float speed = 5.0f;

    void Awake()
    {
        //C# ������ �÷���(�ٷ� ������ ����)
        Destroy(gameObject, 3.0f);   
    }

    void Update()
    {
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self); //Space.Self ������ ��
            //transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World); //Space.World ������ ������        
    }
}
