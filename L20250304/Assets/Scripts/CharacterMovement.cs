using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public float rotateSpeed = 360.0f;

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * v * Time.deltaTime * moveSpeed);
        transform.Rotate(Vector3.up * h * Time.deltaTime * rotateSpeed);
    }
}
