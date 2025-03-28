using UnityEngine;

public class UsingFactory : MonoBehaviour
{
    //�� ���丮 ���
    EnemyFactory enemyFactory = new EnemyFactory(); 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemy enemy = enemyFactory.Create(EnemyFactory.ENEMYTYPE.Goblin);
        enemy.Action();

        Enemy enemy2 = enemyFactory.Create(EnemyFactory.ENEMYTYPE.Slime);
        enemy2.Action();

        Enemy enemy3 = enemyFactory.Create(EnemyFactory.ENEMYTYPE.Wolf);
        enemy3.Action();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
