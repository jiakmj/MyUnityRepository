using System.Collections.Generic;
using Unity.AppUI.UI;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerAttach : MonoBehaviour
{
    private PlayerAnimation playerAnimation;

    public string pickupLayer = "Pickupable";
    public Transform holdPointLeft;
    public Transform holdPointRight;
    public Transform currentHoldPoint;

    private GameObject heldObject = null;
    private GameObject currentPickup = null;
    
    private SpriteRenderer spriteRenderer;

    public Transform sensor;
    public Vector2 offsetRight = new Vector2(0.9f, -0.07f);
    public Vector2 offsetLeft = new Vector2(-1.0f, -0.07f);

    private bool isPushing = false;

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();       
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHoldPoint = holdPointRight;
    }

    void Update()
    {
        UpdateHoldPointDirection();
        UpdateSensorPosition();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Pickup();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Drop();
        }
    }
    private void UpdateHoldPointDirection()
    {
        bool isFacingLeft = spriteRenderer.flipX;
        currentHoldPoint = isFacingLeft ? holdPointLeft : holdPointRight;

        // 만약 들고 있는 오브젝트가 있으면 위치도 같이 갱신
        if (heldObject != null)
        {
            heldObject.transform.SetParent(currentHoldPoint);
            heldObject.transform.localPosition = Vector3.zero;
        }
    }

    private void UpdateSensorPosition()
    {
        bool isFacingLeft = spriteRenderer.flipX;
        sensor.localPosition = isFacingLeft ? offsetLeft : offsetRight;
    }

    private void Pickup()
    {
        if (heldObject != null) 
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(sensor.position, 0.5f);

        foreach (Collider2D hit in hits)
        {
            Pickupable pickupable = hit.GetComponent<Pickupable>();
            if (pickupable != null && pickupable.CanBePickedUp())
            {

                heldObject = hit.gameObject;
                heldObject.transform.SetParent(currentHoldPoint);
                heldObject.transform.localPosition = Vector3.zero;

                Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.simulated = false;
                }

                SetPushState(true);
                break;
            }
        }
    }

    private void Drop()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            playerAnimation.SetPushing(false);

            Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
            }
           
            heldObject = null;
            SetPushState(false);
        }
    }
    private void SetPushState(bool pushing)
    {
        if (isPushing == pushing) return;

        isPushing = pushing;
        playerAnimation.SetPushing(pushing); // 내부에서 animator.SetBool("isPushing", pushing)
    }

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }
}
