using UnityEngine;

public class CreateObject3 : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private int dummy;
    //����ȭ
    //�����ͳ� ������Ʈ�� ��ǻ�� ȯ�濡 �����ϰ� �籸���� �� �ִ� ����(����)�� ��ȯ�ϴ� ����
    //����Ƽ������ �����ϰ� private ������ �����͸� �ν����Ϳ��� ���� �� �ְ� �������شٶ�� �����غ��ô�.

    [SerializeField]
    GameObject sample;

    void Start()
    {
        prefab = Resources.Load<GameObject>("Prefabs/Table_Body");
    }


    void Update()
    {
        //�Է¹��� Ű�� �����̽��� ���
        //GetKeyDown (Ű 1�� �Է�)
        //GetKeyUp (�Է� �� ���� ���)
        //GetKey (������ �ִ� ����)
        if(Input.GetKeyDown(KeyCode.Space))
        {            
            //Resources.Load<T>("������ġ/���¸�");
            //T�� ������ ���¸� �����ִ� ��ġ�Դϴ�.

            //Sprite sprite = Resources.Load<Sprite>("Sprites/sprite01");

            sample = Instantiate(prefab);
            sample.AddComponent<VectorSample>();
            //gameObject.AddComponent<T>
            //������Ʈ�� ������Ʈ ����� �߰��ϴ� ����Դϴ�.
            //GetComponent<T>
            //������Ʈ�� ������ �ִ� ������Ʈ�� ����� ������ ���
            //��ũ��Ʈ���� �ش� ������Ʈ�� ����� �����ͼ� ����ϰ� ���� ��� ����մϴ�.

        }

    }



}
