using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;

    private Vector3 StartPlayerPos;

    private bool isPaused = false;
    public GameObject pauseMenuUI;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        StartPlayerPos = transform.position;
    }

    void Update()
    {
        movement.HandleMovement();

        if (Input.GetButtonDown("Fire1"))
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
        SoundManager.Instance.PlaySFX(SFXType.ItemGet);
    }

    public void ReGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        SoundManager.Instance.PlaySFX(SFXType.ItemGet);
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
            SoundManager.Instance.PlaySFX(SFXType.DamageSound);
            transform.position = StartPlayerPos;
        }
    }
}
