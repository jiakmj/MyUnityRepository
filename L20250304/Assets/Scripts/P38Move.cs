using UnityEngine;

public class P38Move : MonoBehaviour
{
    public float moveSpeed = 30.0f;
     
  
    void Update()
    {      
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
