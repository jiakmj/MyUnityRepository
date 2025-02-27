using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 1.0f;
    Rigidbody2D rd;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rd.linearVelocity = new Vector2(-1, rd.linearVelocity.x);

        if (transform.position.x < -12)
        {
            gameObject.SetActive(false);
        }
    }
  
}
