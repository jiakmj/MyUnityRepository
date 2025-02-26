using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //������ �ð�
    float currentTime;
    
    //���� �ð�
    public float createTime = 1.0f;
    
    //������ ��
    public GameObject enemyFactory;

    float min = 1, max = 5;

    private void Start()
    {
        createTime = Random.Range(min, max);
    }

    private void Update()
    {
        //1. �ð��� �帥��.
        currentTime += Time.deltaTime;
        //2. ���� �ð��� ���� �ð��� �����Ѵٸ� ���� �����մϴ�.
        if (currentTime >= createTime)
        {
            GameObject enemy = Instantiate(enemyFactory);
            enemy.transform.position = transform.position;
            //3. ��ȯ �� �ð��� 0���� �����մϴ�.
            currentTime = 0;
            createTime = Random.Range(min, max);
        }
        
 
    }
}
