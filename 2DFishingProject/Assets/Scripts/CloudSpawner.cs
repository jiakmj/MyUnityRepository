using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public Transform spawnFactory;
    public int minY = -1;
    public int maxY = 4;

    float min = 1, max = 5;

    public int poolSize = 8;
    GameObject[] cloudObjectPool;

    float currentTime;
    public float createTime = 3.0f;

    void Start()
    {
        cloudObjectPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            var cloud = Instantiate(cloudPrefab);
            cloudObjectPool[i] = cloud;
            cloud.SetActive(false);
        }        
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= createTime)
        {            

            for (int i = 0;i < poolSize; i++)
            {
                var cloud = cloudObjectPool[i];
                if (cloud.activeSelf == false)
                {
                    int randomY = Random.Range(minY, maxY);
                    Vector3 spawnPosition = new Vector3(spawnFactory.position.x, randomY, 0);
                    cloud.transform.position = spawnPosition;
                    cloud.SetActive (true);
                    
                    break;
                }                
            }
            currentTime = 0;
            createTime = Random.Range(min, max);
        }

    }
    //IEnumerator SpawnCloud()
    //{
    //    while (true)
    //    {
    //        float randomY = Random.Range(minY, maxY);
    //        Vector3 spawnPosition = new Vector3(spawnFactory.position.x, randomY, 0);
    //        Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
    //        yield return new WaitForSeconds(spawnDeley);
    //    }
    //}

}
