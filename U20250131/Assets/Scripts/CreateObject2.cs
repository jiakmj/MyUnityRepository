using UnityEngine;

public class CreateObject2 : MonoBehaviour
{

    public GameObject prefab;

    private GameObject square;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        square = Instantiate(prefab);

        Destroy(square, 5.0f); //5�� �ڿ� �ı��մϴ�.
        Debug.Log("�ı��Ǿ����ϴ�.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
