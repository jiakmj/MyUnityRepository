using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject pauseUI;
    private bool isPaused = false;

    public Image hpImage;
    public Sprite[] hpSprites;
    private RectTransform hpRectTransform;
    private Vector3 originalPos;

    private int currentHp;
    private int maxHp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;           
        }
        else
        {
            Destroy(gameObject);
        }

        hpRectTransform = hpImage.GetComponent<RectTransform>();
        originalPos = hpRectTransform.anchoredPosition;
    }

    private void Start()
    {
        pauseUI.SetActive(false);
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        SoundManager.Instance.PlaySFX(SFXType.UISound);
    }

    public void Return()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        SoundManager.Instance.PlaySFX(SFXType.UISound);
    }

    public void Exit()
    {
        SoundManager.Instance.PlaySFX(SFXType.UISound);
        Application.Quit();
    }

    // æ¿ ¿¸»Ø Ω√ UI √ ±‚»≠
    public void InitializeUI(int maxHp, int currentHp)
    {
        this.maxHp = maxHp;
        UpdateHP(currentHp);
    }

    public void UpdateHP(int hp)
    {
        hp = Mathf.Clamp(hp, 0, 4);
        hpImage.sprite = hpSprites[hp];

        StopAllCoroutines();
        StartCoroutine(BlinkAndShakeHP());
    }

    private IEnumerator BlinkAndShakeHP()
    {
        Color originalColor = hpImage.color;

        for (int i = 0; i < 5; i++)
        {
            // ±Ù∫˝¿”
            hpImage.color = new Color(1, 1, 1, 0.3f); // ≈ı∏Ì

            // »ÁµÈ∏≤
            Vector2 shakeOffset = Random.insideUnitCircle * 5f;
            hpRectTransform.anchoredPosition = originalPos + (Vector3)shakeOffset;
            yield return new WaitForSeconds(0.05f);

            hpImage.color = originalColor;
            hpRectTransform.anchoredPosition = originalPos;
            yield return new WaitForSeconds(0.05f);
        }

        hpRectTransform.anchoredPosition = originalPos;
        hpImage.color = originalColor;
    }
}
