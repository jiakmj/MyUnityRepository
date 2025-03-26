using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public Image panel;
    public float fadeDuration = 1.0f;
    public string nextSceneName;
    private bool isFading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SoundManager.Instance.SetSFXVolume(0.5f); 
        SoundManager.Instance.PlaySFX("ButtonSound", transform.position); //UI클릭 사운드
        //딜레이도 넣고
        SceneManager.LoadScene("map2");

        Debug.Log("씬 이동 : " + sceneName);
    }

    public void ExitScene()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        Debug.Log("게임 오버");
        SceneManager.LoadScene("Game Over");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isFading)
        {
            StartCoroutine(FadeInAndLoadScene());

        }
    }

    IEnumerator FadeInAndLoadScene()
    {
        isFading = true;

        yield return StartCoroutine(FadeImage(0, 1, fadeDuration));

        SceneManager.LoadScene(nextSceneName);

        yield return StartCoroutine(FadeImage(1, 0, fadeDuration));

        isFading = false;
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0.0f;

        Color panelColor = panel.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            panelColor.a = newAlpha;
            panel.color = panelColor;
            yield return null;
        }
        panelColor.a = endAlpha;
        panel.color = panelColor;

        if (isFading)
        {
            SceneManager.LoadScene(nextSceneName);

        }
    }
}
