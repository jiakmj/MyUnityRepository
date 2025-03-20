using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private Dictionary<string, AudioClip> Dic_bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> Dic_sfxClips = new Dictionary<string, AudioClip>();

    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudioClip[] bgmClipList;
    public NamedAudioClip[] sfxClipList;

    private Coroutine currentBGMCoroutine;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        OnSceneLoaded(activeSceneName);
    }

    public void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "GameScene")
        {
            PlayBGM("GameScene1", 1.0f);
        }
        else if (sceneName == "GameScene2")
        {
            PlayBGM("GameScene2", 1.0f);
        }
    }

    void InitializeAudioClips()
    {
        foreach (var bgm in bgmClipList)
        {
            if (!Dic_bgmClips.ContainsKey(bgm.name))
            {
                Dic_bgmClips.Add(bgm.name, bgm.clip);
            }
        }
        foreach (var sfx in sfxClipList)
        {
            if (!Dic_sfxClips.ContainsKey(sfx.name))
            {
                Dic_sfxClips.Add(sfx.name, sfx.clip);
            }
        }
    }
    
    public void PlayBGM(string name, float fadeDuration = 1.0f)
    {
        if (Dic_bgmClips.ContainsKey(name))
        {
            if (currentBGMCoroutine != null) //실행중이라면 오류나니까 멈춰줌
            {
                StopCoroutine(currentBGMCoroutine);
            }
            currentBGMCoroutine = StartCoroutine(FadeOutBGM(fadeDuration, () =>
            {
                bgmSource.spatialBlend = 0f;
                
                bgmSource.clip = Dic_bgmClips[name];
                bgmSource.Play();
                currentBGMCoroutine = StartCoroutine(FadeInBGM(fadeDuration));
            }));            
        }
    }

    public void PlaySFX(string name, Vector3 position)
    {
        if (Dic_sfxClips.ContainsKey(name))
        {
            AudioSource.PlayClipAtPoint(Dic_sfxClips[name], position);
            //sfxSource.PlayOneShot(Dic_sfxClips[name], position);         
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }


    //볼륨 크기 조절
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp(volume, 0, 1);
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp(volume, 0, 1);
    }

    //점점 소리를 줄이고
    private IEnumerator FadeOutBGM(float duration, Action onFadeComplete)
    {
        float startVolume = bgmSource.volume;

        for (float t = 0; t < duration; t++)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        bgmSource.volume = 0;
        onFadeComplete?.Invoke(); //페이드 아웃이완료되면 다음 작업 실행
    }

    //점점 소리를 키우고
    private IEnumerator FadeInBGM(float duration)
    {
        float startVolume = 0f;
        bgmSource.volume = 0f;
        
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 1f, t / duration);
            yield return null;
        }
        bgmSource.volume = 1.0f;
    }
}
