using System;
using UnityEngine;

//������Ʈ���� ������ �Ŵ���(����)
public class Manager : MonoBehaviour
{
    //�̱��� ����
    public static Manager instance;

    private static PoolManager pool_manager = new PoolManager();
    public static PoolManager POOL
    {
        get
        {
            return pool_manager;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Resources ������ ����� �ʿ��� �ڵ�
    public GameObject CreateFromPath(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }
}
