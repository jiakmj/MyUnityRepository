using UnityEngine;

public class Bullet : MonoBehaviour
{    
    float speed = 5.0f;

    void Awake()
    {
        //C# 가비지 컬렉터(바로 지우지 않음)
        Destroy(gameObject, 3.0f);   
    }

    void Update()
    {
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self); //Space.Self 내꺼의 위
            //transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World); //Space.World 세상의 위에서        
    }
}
