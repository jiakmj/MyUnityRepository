using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

//���带 �̿��ؼ� ������� ȭ�鿡�� ǥ���ϴ� ����
//AudioUI ������Ʈ�� ����
public class AudioBoardVisualizer : MonoBehaviour
{
    //���� ������
    public float minBoard = 50.0f;
    public float maxBoard = 500.0f;

    //����� ����� Ŭ��
    public AudioClip audioClip;
    //����� ����� �ҽ�
    public AudioSource audioSource;

    //����� �ͼ� ����
    public AudioMixer audioMixer;

    public Board[] boards;

    //����Ʈ���� samples
    public int samples = 64;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boards = GetComponentsInChildren<Board>();
        //GetComponentsInChildren<T>�� ������Ʈ�� ����� �ڽ� ������Ʈ���� �������� �ڵ�
        //���� �ڵ� �������δ� Board�� �迭

        //== ���� ������Ʈ�� ���� ������Ʈ�� ������ִ� �ڵ� ==
        //����� �ҽ��� null�̶��
        if (audioSource == null )
        {
            //"AudioSource" ���� ������Ʈ�� �����ϰ�, �ش� ������Ʈ�� AudioSource ������Ʈ�� �߰��ϰڽ��ϴ�.
            audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();       
        }
        else
        {
            //�����ϸ� ������ ã�Ƽ� ����ϰڽ��ϴ�.
            audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();            
        }
        audioSource.clip = audioClip;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
        //����� �ͼ� �׷� �߿��� "Master" �׷��� ã�� �����մϴ�.
        audioSource.Play();
       
    }
    



    // Update is called once per frame
    void Update()
    {
        float[] datas = audioSource.GetSpectrumData(samples, 0, FFTWindow.Rectangular);
        //GetSpectrumData(samples, channel, FFTWindow);

        //samples = FFT(��ȣ�� ���� ���ļ� ����)�� ������ �迭, �� �迭 ���� 2�� ���� ���� ǥ���մϴ�.
        //ä���� �ִ� 8���� ä���� �������ְ� ����. �Ϲ������δ� 0�� ����մϴ�.
        //FFTWindow�� ���ø� ������ �� ���� ��

        //���� ������ ������ŭ �۾��� �����մϴ�.
        for (int i = 0; i < boards.Length; i++)
        {
            var size = boards[i].GetComponent<RectTransform>().rect.size;
            //���� ������ ������ �ִ� ����� �����ڽ��ϴ�.

            size.y = minBoard + (datas[i] * (maxBoard - minBoard) * 3.0f);
            //���⼭ 3.0f�� ���뿡 ���� ��ġ ���� ��

            boards[i].GetComponent<RectTransform>().sizeDelta = size;
            //sizeDelta�� �θ� �������� ũ�Ⱑ �󸶳� ū�� �������� ��Ÿ�� ��ġ�� �ǹ��մϴ�.

        }


    }
}
