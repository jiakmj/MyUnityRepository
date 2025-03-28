using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSourceSample : MonoBehaviour
{
    //0) 인스펙터에서 직접 연결하는 경우
    public AudioSource audioSourceBGM;    

    //1) AudioSourceSample을 객체가 자체적으로 오디오 소스를 가지고 있는 경우
    //private AudioSource own_audioSource;

    //3) 씬에서 찾아서 연결하는 경우
    //4) Resources.Load() 기능을 이용해 리소스 폴더에 있는 오디오 소스의 클립을 받아오겠습니다.
    //public AudioSource audioSourceSFX;

    public AudioClip bgm; //오디오 클립에 대한 연결

    public Button playButton;
    public TextMeshProUGUI buttonText;
    
    public bool playing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //1)의 경우 GetComponent<T>를 통해 해당 객체가 가지고 있는 오디오 소스 연결 가능
        //own_audioSource = GetComponent<AudioSource>(); //지금은 BGM으로 따로 빼놓아서 X

        //3)의 경우 GameObject.Find().GetComponent<T> 활용
        //GameObject.Find()는 씬에서 찾은 gameObject를 return하는 기능을 가지고 있음. 즉 이 값은 gameObject임
        //GameObject이기 때문에 GetComponent<T>를 이어 작성함으로써 오브젝트가 가진 컴포넌트의 값을 return 합니다.
        //따라서 저 결과물은 AudioSource가 됩니다.
        //audioSourceSFX = GameObject.Find("SFX").GetComponent<AudioSource>();

        audioSourceBGM.clip = bgm;
        //오디오 소스의 클립을 bgm으로 설정합니다.

        //audioSourceSFX.clip = Resources.Load("Explosion") as AudioClip;
        //Resources.Load()는 리소스 폴더에서 오브젝트를 찾아 로드하는 기능입니다.
        //이때 받아오는 값은 Object이므로 as 클래스명을 통해 해당 데이터가 어떤 형태인지 표현해주면 그 형태로 받아오게 됩니다.

        //audioSourceSFX.clip = Resources.Load("Audio/BombCharge") as AudioClip;
        //리소스 로드 시 경로가 정해져 있다면 폴더명/파일명으로 작성합니다.
        //리소스 로드 시 작성하는 이름에는 확장자명(ex) .json, .avi)를 작성하지 않습니다.

        //UnityWebRequest의 GetAudiocClip 기능 활용
        //StartCoroutine("GetAudioClip");

        //오디오 소스 스크립트 기능
        //audioSourceBGM.Play(); //클립을 실행하는 도구
        //audioSourceBGM.Pause(); //일시 정지 기능
        //audioSourceSFX.PlayOneShot(bgm); //클립 하나를 한순간 플레이를 진행합니다.
        //audioSourceBGM.Stop(); //오디오 클립 재생 중지
        //audioSourceBGM.UnPause(); //일시 정지 해제
        //audioSourceBGM.PlayDelayed(1.0f); //1초 뒤에 재생

        if (playButton != null )
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(bgmstart);
        }       
    }

    //IEnumerator GetAudioClip()
    //{
    //    UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file:///" + Application.dataPath + "/Audio/" + "Explosion" + ".wav", AudioType.WAV);
    //    yield return uwr.SendWebRequest(); //전달

    //    var new_clip = DownloadHandlerAudioClip.GetContent(uwr);
    //    //받은 경로를 기반으로 다운로드 진행

    //    audioSourceBGM.clip = new_clip; //클립 등록
    //    audioSourceBGM.Play(); //플레이
    //}
    //이걸로 호출할 경우 작업 끝나면 값 사라짐.

    public void bgmstart()
    {
        if (playing)
        {
            playing = false;
            audioSourceBGM.Pause();
            buttonText.text = "Play";

            Debug.Log("음악이 멈췄습니다. 현재 상태: " + playing);
            
        }
        else
        {
            playing = true;            
            audioSourceBGM.Play();
            buttonText.text = "Pause";

            Debug.Log("음악이 재생 중입니다. 현재 상태: " + playing);
        }             
    }

    
 
}
