using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootRange = 20f;
    public float damage = 1f;

    public LayerMask hitLayerMask;
    //public ParticleType hitParticle;

    public Transform shootOrigin;

    public SpriteRenderer playerSpriteRenderer;
    
    public void Fire()
    {
        if (playerSpriteRenderer == null)
        {
            return;
        }

        Vector2 direction = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 origin = shootOrigin.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, shootRange, hitLayerMask);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);

            EnemyManager enemy = hit.collider.GetComponent<EnemyManager>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            //ParticleManager.Instance.ParticlePlay(hitParticle, hit.point, Vector3.one * 2f);
        }
        else
        {
            Debug.Log("Missed");
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (shootOrigin != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Vector3 direction = transform.localScale.x < 0? Vector3.left : Vector3.right;

    //        Gizmos.DrawLine(shootOrigin.position, shootOrigin.position + direction * shootRange);
    //    }
    //}
}
