using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

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
        SoundManager.Instance.PlaySFX("ButtonSound", transform.position); //UIŬ�� ����
        //�����̵� �ְ�
        SceneManager.LoadScene("map2");

        Debug.Log("�� �̵� : " + sceneName);
    }

    public void ExitScene()
    {
        Application.Quit();
    }
}
