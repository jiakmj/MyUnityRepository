using UnityEngine;

public class P38Rotator : MonoBehaviour
{
    public float rotatinSpeed = 60.0f;

  
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Rotate(new Vector3(v, 0, -h).normalized * rotatinSpeed * Time.deltaTime);
    }
}
