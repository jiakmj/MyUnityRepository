using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //public GameObject BulletArmo;
    //public Transform[] FirePoint;

    //public List<Transform> transforms = new List<Transform>();

    float moveSpeed = 3.0f;
    float rotationSpeed = 60.0f;

    void Awake()
    {
        //FirePoint = GameObject.Find("FirePoint").GetComponentsInChildren<Transform>();
        //FirePoint = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Transform>();
        //Find는 추천하지 않음 오타문제와 오브젝트가 많으면 효율이 떨어짐 tag찾는건 괜찮음

        //transform.GetChild()
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(0, v, 0).normalized * Time.deltaTime * moveSpeed); //normalized 안 하면 대각선으로 갈 때 더 빠름
        transform.Rotate(new Vector3(0, 0, -h) * Time.deltaTime * rotationSpeed);

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    for (int i = 0; i < FirePoint.Length; i++)
        //    {
        //        GameObject newGameObject = Instantiate(BulletArmo);
        //        newGameObject.transform.position = FirePoint[i].position;
        //        newGameObject.transform.rotation = FirePoint[i].rotation;
        //    }

        //    //foreach (var point in FirePoint)
        //    //{
        //    //    {
        //    //        GameObject newGameObject = Instantiate(BulletArmo);
        //    //        newGameObject.transform.position = point.position;
        //    //        newGameObject.transform.rotation = point.rotation;
        //    //    }
        //    //}
        //}
    }
}
