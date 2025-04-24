using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private GameObject playerInRange = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHold"))
        {
            isPlayerInRange = true;
            playerInRange = collision.transform.root.gameObject; // 플레이어 본체
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHold"))
        {
            isPlayerInRange = false;
            playerInRange = null;
        }
    }

    public bool CanBePickedUp() => isPlayerInRange;
    public GameObject GetPlayerInRange() => playerInRange;
}