using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource; //����� ����� AudioSource
    public AudioSource sfxSource; //ȿ���� ����� AudioSource

    public Dictionary<BGMType, AudioClip> bgmDic = new Dictionary<BGMType, AudioClip>();
    public Dictionary<SFXType, AudioClip> sfxDic = new Dictionary<SFXType, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //���� ���� �� �ڵ����� ����Ǵ� �ʱ�ȭ �Լ�
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitSoundManager()
    {
        GameObject obj = new GameObject("SoundManager");
        Instance = obj.AddComponent<SoundManager>();
        DontDestroyOnLoad(obj);

        //BGM����
        GameObject bgmObj = new GameObject("BGM");
        SoundManager.Instance.bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmObj.transform.SetParent(obj.transform);
        SoundManager.Instance.bgmSource.loop = true;
        SoundManager.Instance.bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);

        //SFX����
        GameObject sfxObj = new GameObject("SFX");
        SoundManager.Instance.sfxSource = sfxObj.AddComponent<AudioSource>();
        SoundManager.Instance.sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxObj.transform.SetParent(obj.transform); //�θ� ���� ����

        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Sound/BGM"); //BGM ���ҽ� �ε�

        foreach (var clip in bgmClips)
        {
            try
            {
                BGMType type = (BGMType)Enum.Parse(typeof(BGMType), clip.name);
                SoundManager.Instance.bgmDic.Add(type, clip);
            }
            catch 
            { 
                Debug.LogWarning("BGM Enum �ʿ� : " + clip.name);
            }
        }

        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Sound/SFX"); //SFX ���ҽ� �ε�

        foreach (var clip in sfxClips)
        {
            try
            {
                SFXType type = (SFXType)Enum.Parse(typeof(SFXType), clip.name);
                SoundManager.Instance.sfxDic.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("SFX Enum �ʿ� : " + clip.name);
            }
        }
        //�� �ε��� ������ OnSceneLoadCompleted ȣ��
        SceneManager.sceneLoaded += SoundManager.Instance.OnSceneLoadCompleted;
    }

    //�� ��ȯ �Ϸ� �� �ڵ� ȣ��Ǵ� �Լ�
    public void OnSceneLoadCompleted(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TestScene") 
        {
            PlayBGM(BGMType.Track1, 0.2f);
        }
        else if (scene.name == "Tutorial")
        {
            PlayBGM(BGMType.Track2, 1f);
        }
    }

    public void PlaySFX(SFXType type) //ȿ���� ���
    {
        sfxSource.PlayOneShot(sfxDic[type]);
    }

    public void PlayBGM(BGMType type, float fadeTime = 0) //����� ��� �Լ�(���̵� ȿ�� ����)
    {
        if (bgmSource.clip != null)
        {
            if (bgmSource.clip.name == type.ToString())
            {
                return;
            }

            if (fadeTime == 0)
            {
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
            }
            else
            {
                StartCoroutine(FadeOutBGM(() =>
                    {
                        bgmSource.clip = bgmDic[type];
                        bgmSource.Play();
                        StartCoroutine(FadeInBGM(fadeTime));
                    }, fadeTime));
            }
        }
        else
        {
            if (fadeTime == 0)
            {
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
            }
            else
            {
                bgmSource.volume = 0;
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
                StartCoroutine(FadeInBGM(fadeTime));
            }
        }
    }

    private IEnumerator FadeOutBGM(Action onComplete, float duration) //BGM ������ õõ�� ���̴� �ڷ�ƾ
    {
        float startVolume = bgmSource.volume;
        float time = 0;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = 0f;
        onComplete?.Invoke(); //���̵� �ƿ� �� �ݹ� ����
    }

    private IEnumerator FadeInBGM(float duration = 1.0f) //BGM ������ õõ�� �ø��� �ڷ�ƾ
    {
        float targetVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float time = 0;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    public void SetBGMVolume(float volume) //BGM ���� ����
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume) //SFX ���� ����
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume );
    }   
}

public enum BGMType //BGM ����
{
    Track1, Track2, Track3, Track4, Track5, Track6, Track7, Track8, Track9, Track10,
}

public enum SFXType //SFX ����
{
    Attack, HitSound, DamageSound, DeadSound, JumpSound, StepSound, Heal, ItemGet, UISound, Slam

}
