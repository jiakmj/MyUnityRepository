using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;

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

        // �� �� �ε� �Ϸ� �� panel �ٽ� ã��
        yield return new WaitUntil(() => panel != null); // <- �� �� �ε� �� �ٽ� ���
        panel.gameObject.SetActive(true); // �ٽ� Ȱ��ȭ

        yield return new WaitUntil(() => UIManager.Instance != null);

        if (sceneName == "Tutorial" || sceneName == "GameScene1")
        {
            UIManager.Instance.InitializeUI(true, true);
        }
        else if (sceneName == "Menu")
        {
            UIManager.Instance.InitializeUI(false, false);
        }

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
        Debug.Log("���ӽ���");
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

        if (UIManager.Instance == null)
        {
            UIManager.Instance = FindObjectOfType<UIManager>();
            Debug.Log("UIManager ã��: " + (UIManager.Instance != null));
        }

        Debug.Log("[PlayerState] �� �ε��. HP �ʱ�ȭ!");

        // ī�޶� �翬��
        var virtualCam = FindObjectOfType<CinemachineCamera>();
        if (virtualCam != null)
        {
            virtualCam.Follow = PlayerState.Instance.transform;
            virtualCam.LookAt = PlayerState.Instance.transform;
            Debug.Log("[Cinemachine] Follow, LookAt �翬�� �Ϸ�");
        }
        else
        {
            Debug.LogWarning("[Cinemachine] VirtualCamera�� ã�� �� �����ϴ�.");
        }

    }
}
