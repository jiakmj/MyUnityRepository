using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;

    
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        
    }
}
