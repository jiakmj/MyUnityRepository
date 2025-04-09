using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isInvincible = false;
    public float invincibilityDuration = 1.0f;
    public float knockbackForce = 5.0f;
    private Rigidbody2D rb;
    private bool isKnockback = false;
    public float knockbackDuration = 0.2f;

    private Vector3 StartPlayerPos;
    private bool isPaused = false;
    public GameObject pauseMenuUI;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartPlayerPos = transform.position;
    }

    void Update()
    {
        if (!isKnockback)
        {
            movement.HandleMovement();
        }

        if (Input.GetButtonDown("Fire1") && !isKnockback)
        {
            attack.PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ReGame();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        SoundManager.Instance.PlaySFX(SFXType.UISound);
    }

    public void ReGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        SoundManager.Instance.PlaySFX(SFXType.UISound);
    }

    public void MenuOn()
    {
        SoundManager.Instance.PlaySFX(SFXType.UISound);
        //SoundManager.LoadScene("Menu");
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.Instance.AddCoin(10);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("DeadZone"))
        {
            SoundManager.Instance.PlaySFX(SFXType.DeadSound);
            transform.position = StartPlayerPos;
        }
        else if (collision.CompareTag("Monster"))
        {
            PlayerAttack playerAttack = GetComponent<PlayerAttack>();
            float shakeDuration = 0.1f;
            float shakeMagnitude = 0.3f;
            //StartCoroutine(playerAttack.Shake(shakeDuration, shakeMagnitude));

            if (!isInvincible)
            {
                SoundManager.Instance.PlaySFX(SFXType.HitSound);
                StartCoroutine(Invincibility());

                Vector2 knockbackDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                animator.SetTrigger("Hit");
                StartCoroutine(KnockbackCoroutine());                
            }
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        Time.timeScale = 0.8f;
        float elapsedTime = 0f;
        float blinkInterval = 0.2f;

        Color originalColor = spriteRenderer.color;

        while(elapsedTime < invincibilityDuration)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.4f);
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval * 2;

        }
        spriteRenderer.color = originalColor;
        isInvincible = false;
    }

    IEnumerator KnockbackCoroutine()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
    }
}
