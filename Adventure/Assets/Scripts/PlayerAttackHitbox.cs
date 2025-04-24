using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            EnemyManager enemy = collision.GetComponent<EnemyManager>();
            if (enemy != null)
            {
                int playerDamage = PlayerState.Instance.GetDamage();
                enemy.TakeDamage(playerDamage);
            }
        }
    }

}
