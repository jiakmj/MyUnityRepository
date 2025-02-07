using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSourceSample : MonoBehaviour
{
    //0) �ν����Ϳ��� ���� �����ϴ� ���
    public AudioSource audioSourceBGM;    

    //1) AudioSourceSample�� ��ü�� ��ü������ ����� �ҽ��� ������ �ִ� ���
    //private AudioSource own_audioSource;

    //3) ������ ã�Ƽ� �����ϴ� ���
    //4) Resources.Load() ����� �̿��� ���ҽ� ������ �ִ� ����� �ҽ��� Ŭ���� �޾ƿ��ڽ��ϴ�.
    //public AudioSource audioSourceSFX;

    public AudioClip bgm; //����� Ŭ���� ���� ����

    public Button playButton;
    public TextMeshProUGUI buttonText;
    
    public bool playing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //1)�� ��� GetComponent<T>�� ���� �ش� ��ü�� ������ �ִ� ����� �ҽ� ���� ����
        //own_audioSource = GetComponent<AudioSource>(); //������ BGM���� ���� �����Ƽ� X

        //3)�� ��� GameObject.Find().GetComponent<T> Ȱ��
        //GameObject.Find()�� ������ ã�� gameObject�� return�ϴ� ����� ������ ����. �� �� ���� gameObject��
        //GameObject�̱� ������ GetComponent<T>�� �̾� �ۼ������ν� ������Ʈ�� ���� ������Ʈ�� ���� return �մϴ�.
        //���� �� ������� AudioSource�� �˴ϴ�.
        //audioSourceSFX = GameObject.Find("SFX").GetComponent<AudioSource>();

        audioSourceBGM.clip = bgm;
        //����� �ҽ��� Ŭ���� bgm���� �����մϴ�.

        //audioSourceSFX.clip = Resources.Load("Explosion") as AudioClip;
        //Resources.Load()�� ���ҽ� �������� ������Ʈ�� ã�� �ε��ϴ� ����Դϴ�.
        //�̶� �޾ƿ��� ���� Object�̹Ƿ� as Ŭ�������� ���� �ش� �����Ͱ� � �������� ǥ�����ָ� �� ���·� �޾ƿ��� �˴ϴ�.

        //audioSourceSFX.clip = Resources.Load("Audio/BombCharge") as AudioClip;
        //���ҽ� �ε� �� ��ΰ� ������ �ִٸ� ������/���ϸ����� �ۼ��մϴ�.
        //���ҽ� �ε� �� �ۼ��ϴ� �̸����� Ȯ���ڸ�(ex) .json, .avi)�� �ۼ����� �ʽ��ϴ�.

        //UnityWebRequest�� GetAudiocClip ��� Ȱ��
        //StartCoroutine("GetAudioClip");

        //����� �ҽ� ��ũ��Ʈ ���
        //audioSourceBGM.Play(); //Ŭ���� �����ϴ� ����
        //audioSourceBGM.Pause(); //�Ͻ� ���� ���
        //audioSourceSFX.PlayOneShot(bgm); //Ŭ�� �ϳ��� �Ѽ��� �÷��̸� �����մϴ�.
        //audioSourceBGM.Stop(); //����� Ŭ�� ��� ����
        //audioSourceBGM.UnPause(); //�Ͻ� ���� ����
        //audioSourceBGM.PlayDelayed(1.0f); //1�� �ڿ� ���

        if (playButton != null )
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(bgmstart);
        }       
    }

    //IEnumerator GetAudioClip()
    //{
    //    UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file:///" + Application.dataPath + "/Audio/" + "Explosion" + ".wav", AudioType.WAV);
    //    yield return uwr.SendWebRequest(); //����

    //    var new_clip = DownloadHandlerAudioClip.GetContent(uwr);
    //    //���� ��θ� ������� �ٿ�ε� ����

    //    audioSourceBGM.clip = new_clip; //Ŭ�� ���
    //    audioSourceBGM.Play(); //�÷���
    //}
    //�̰ɷ� ȣ���� ��� �۾� ������ �� �����.

    public void bgmstart()
    {
        if (playing)
        {
            playing = false;
            audioSourceBGM.Pause();
            buttonText.text = "Play";

            Debug.Log("������ ������ϴ�. ���� ����: " + playing);
            
        }
        else
        {
            playing = true;            
            audioSourceBGM.Play();
            buttonText.text = "Pause";

            Debug.Log("������ ��� ���Դϴ�. ���� ����: " + playing);
        }             
    }

    
 
}
