using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bulletFactory; //�Ѿ� ������
    public GameObject firePosition; //�� �߻� ��ġ

    #region ObjectPool
    public int poolSize = 10;

    GameObject[] bulletObjectPool;

    #endregion

    private void Start()
    {
        //1. ������ ũ�⸸ŭ Ǯ�� ������Ʈ ����
        bulletObjectPool = new GameObject[poolSize];

        //2. ����ŭ �ݺ��� �Ѿ� ����
        for (int i = 0; i < poolSize; i++)
        {
            //�Ѿ� ����
            var bullet = Instantiate(bulletFactory);
            //Ǯ�� ���
            bulletObjectPool[i] = bullet;
            //��Ȱ��ȭ(�ʿ��� �� Ȱ��ȭ�մϴ�.)
            bullet.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //Fire1 = Left Ctrl
        {
            for (int i = 0; i < poolSize; i++)
            {
                var bullet = bulletObjectPool[i];

                if (bullet.activeSelf == false)
                {
                    bullet.SetActive(true);
                    bullet.transform.position = firePosition.transform.position;
                    break;
                }
            }

        }
    }
}
