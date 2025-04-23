using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public Image panel;
    public float fadeDuration = 1.0f;
    private bool isFading = false;

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

    public void StartSceneTransition(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeInAndLoadScene(sceneName));
        }
    }

    private IEnumerator FadeInAndLoadScene(string sceneName)
    {
        isFading = true;

        yield return new WaitUntil(() => panel != null);

        panel.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(0, 1, fadeDuration));

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = true;

        yield return asyncOperation;

        // 씬 전환 완료 후 다시 panel 연결 (중요!!!)
        yield return new WaitUntil(() => panel != null);  // 씬 전환 직후 새로 연결될 수 있음

        // 씬에 맞는 UI 초기화
        UIManager.Instance.InitializeUI(4, 4); // 예시 값, 필요에 따라 수정

        yield return StartCoroutine(FadeImage(1, 0, fadeDuration));
        panel.gameObject.SetActive(false);
        isFading = false;
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
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
    }

    public void ExitScene()
    {
        Application.Quit();
    }

    public void GameStart()
    {
        Debug.Log("게임시작");
        StartSceneTransition("Tutorial");
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (panel == null)
        {
            panel = GameObject.Find("FadePanel")?.GetComponent<Image>();
            if (panel == null)
            {
                Debug.LogWarning("FadePanel not found in the scene.");
            }
        }
    }
}
