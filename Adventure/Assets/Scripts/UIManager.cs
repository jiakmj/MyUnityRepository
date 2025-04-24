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

    [Header("UI Coin Icons (√÷¥Î 3∞≥)")]
    public Image[] coinImages;

    public GameObject coinCompleteText;
    private Color[] originalColors;

    private Coroutine hideTextCoroutine;

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

        originalColors = new Color[coinImages.Length];
        for (int i = 0; i < coinImages.Length; i++)
        {
            originalColors[i] = coinImages[i].color;
            coinImages[i].color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0.3f);
        }

        if(coinCompleteText != null)
        {
            coinCompleteText.SetActive(false);
        }
        
    }

    private void Start()
    {
        pauseUI.SetActive(false);

    }

    public void UpdateCoinUI(int totalCoin)
    {
        for (int i = 0; i < coinImages.Length; i++)
        {
            if (i < totalCoin)
                coinImages[i].color = originalColors[i]; // ø¯∑° ªˆªÛ ∫πø¯
            else
                coinImages[i].color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0.3f); // »Â∏¥«œ∞‘
        }

        if (totalCoin >= 3 && coinCompleteText != null)
        {
            coinCompleteText.SetActive(true);
            if (hideTextCoroutine != null)
                StopCoroutine(hideTextCoroutine);

            hideTextCoroutine = StartCoroutine(HideCoinTextAfterSeconds(2f));
        }
    

    }
    IEnumerator HideCoinTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (coinCompleteText != null)
            coinCompleteText.SetActive(false);
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
