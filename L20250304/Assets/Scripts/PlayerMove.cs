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
        //Find�� ��õ���� ���� ��Ÿ������ ������Ʈ�� ������ ȿ���� ������ tagã�°� ������

        //transform.GetChild()
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(0, v, 0).normalized * Time.deltaTime * moveSpeed); //normalized �� �ϸ� �밢������ �� �� �� ����
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
