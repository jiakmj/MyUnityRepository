using UnityEngine;

public enum EnemyType
{
    WalkSlime, FlyingSlime, RedSlime, BlueSlime, GreenSlime, PinkSlime, BossSlime,
}

public class AIManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnRangeX = 10.0f;
    public float spawnRangeY = 5.0f;
    public int enemyCount = 5;
    public Transform[] spawnPoints;
    private float monsterSpeed = 1.0f;
    private float monsterHp = 1.0f;
    private float monsterDamage = 1.0f;
    private EnemyType currentEnemyType = EnemyType.WalkSlime;
    
    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Vector2 spawnPosition = spawnPoints[randomIndex].position;
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                Vector2 randomPosition = new Vector2(randomX, randomY);
                Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
            }
        }
    }

    void EnemySetState()
    {
        EnemyManager monster = monsterPrefab.GetComponent<EnemyManager>();
        float minSpeed = 1f;
        float maxSpeed = 10f;
        float minHp = 1f;
        float maxHp = 10f;
        float minDamage = 1f;
        float maxDamage = 10f;

        if ( currentEnemyType == EnemyType.WalkSlime)
        {
            minSpeed = 1.0f;
            maxSpeed = 5.0f;
            minHp = 1.0f;
            maxHp = 10.0f;
            minDamage = 1.0f;
            maxDamage = 10.0f;
        }
        else if (currentEnemyType == EnemyType.FlyingSlime)
        {
            minSpeed = 3.0f;
            maxSpeed = 7.0f;
            minHp = 3.0f;
            maxHp = 10.0f;
            minDamage = 1.0f;
            maxDamage = 5.0f;
        }
        else if (currentEnemyType == EnemyType.RedSlime)

        {
            minSpeed = 0.5f;
            maxSpeed = 3.0f;
            minHp = 5.0f;
            maxHp = 15.0f;
            minDamage = 7.0f;
            maxDamage = 20.0f;
        }
        monsterSpeed = Random.Range(minSpeed, maxSpeed);
        monsterHp = Random.Range(minHp, maxHp);
        monsterDamage = Random.Range(minDamage, maxDamage);
        monster.speed = monsterSpeed;
        monster.hp = monsterHp;
        monster.damage = monsterDamage;

    }


    // 경로 그리기
    private void OnDrawGizmosSelected()
    { 
        Gizmos.color= Color.green;
        Gizmos.DrawWireCube(Vector2.zero, new Vector2(spawnRangeX * 2, spawnRangeY * 2));
        Gizmos.color = Color.blue;
        if (spawnPoints.Length > 0)
        {
            foreach(Transform spawnPoint in spawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
