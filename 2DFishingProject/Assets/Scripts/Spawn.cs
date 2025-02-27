using UnityEngine;

public class Spawner : MonoBehaviour
{
    //������ �ð�
    float currentTime;

    //���� �ð�
    public float createTime = 1.0f;

    //������ ��
    public GameObject cloudFactory;

    float min = 1, max = 5;

    //������Ʈ Ǯ ����
    public int poolSize = 10;
    GameObject[] cloudObjectPool;

    //���� ��ġ(�迭)
    public Transform[] spawnPoints;

    private void Start()
    {
        createTime = Random.Range(min, max);

        cloudObjectPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            var enemy = Instantiate(cloudFactory);
            cloudObjectPool[i] = enemy;
            enemy.SetActive(false);
        }
    }

    private void Update()
    {
        //1. �ð��� �帥��.
        currentTime += Time.deltaTime;
        //2. ���� �ð��� ���� �ð��� �����Ѵٸ� ���� �����մϴ�.
        if (currentTime >= createTime)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var enemy = cloudObjectPool[i];
                if (enemy.activeSelf == false)
                {
                    //���� ����
                    int index = Random.Range(0, spawnPoints.Length);
                    enemy.transform.position = spawnPoints[index].position;
                    enemy.SetActive(true);
                    break;
                }
            }
            //3. ��ȯ �� �ð��� 0���� �����մϴ�.
            currentTime = 0;
            createTime = Random.Range(min, max);
        }
    }



    //public float minSpawnDelay = 1.0f;
    //public float maxSpawnDelay = 2.0f;

    //[Header("References")]
    //public GameObject[] gameObjects;


    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void OnEnable()
    //{
    //    Invoke("Spawn", Random.Range(minSpawnDelay, maxSpawnDelay));
    //}

    //void OnDisable()
    //{
    //    CancelInvoke();
    //}

    //void Spawn()
    //{
    //    GameObject randomObject = gameObjects[Random.Range(0, gameObjects.Length)];
    //    Instantiate(randomObject, transform.position, Quaternion.identity);
    //    Invoke("Spawn", Random.Range(minSpawnDelay, maxSpawnDelay));
    //}

}
