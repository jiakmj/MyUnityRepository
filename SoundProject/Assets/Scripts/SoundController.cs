using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//����Ƽ �����Ϳ��� GUI�� �����ִ� �ý���
//IMGUI: ����׿��� ���

//UGUI: �Ϲ������� ����ϴ� UI ����

//UIElements: ������ ������� �����ϴ� ����

//using UnityEngine.UIElements;


//�����̴�(Slider)�� UI
//�ڵ��ϼ����� ��������� �����̴� ����Ʈ 2D(SliderJoint 2D)�� ����
//Rigidbody ������� �޴� ���� ������Ʈ�� �������� ���� ���� �̲������� �ϴ� ������ �� �� ���
//ex) �̴��̹�


public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider BGMVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    private void Awake()
    {
        MasterVolumeSlider.onValueChanged.AddListener(SetMaster);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGM);
        SFXVolumeSlider.onValueChanged.AddListener(SetSFX);
        
    }

    private void SetSFX(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume)* 20);
    }

    private void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    private void SetMaster(float volume)
    {
        //������ͼ��� ������ �ִ� �Ķ����(Expose)�� ��ġ�� �����ϴ� ���
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

        //���� ���Ǵ� Mathf �Լ�
        //1. Mathf.Deg2Rad
        // degree(60�й�)�� ���� radian(ȣ����)�� ����ϴ� �ڵ�
        // --> ���� ��� �ڵ�
        // PI / 180, PI * 2 / 360

        //2. Mathf.Rad2Deg
        //���� ���� ��׸� ������ �ٲ��ݴϴ�.
        // 360 / (pi * 2)
        //1 ������ �� 57��

        //3. Mathf.Abs()
        //���밪�� ������ִ� ���
        
        //4. Mathf.Atan
        //��ũ ź��Ʈ ���� ����մϴ�.

        //5. Mathf.Ceil(��)
        //�Ҽ��� �ø� ���

        //6. Mathf.Clamp(value, min, max)
        //value�� min�� max ������ ������ �����ϴ� ���

        //7. Mathf.Log10
        //���̽��� 10���� �����Ǿ� �ִ� ���� �α׸� ��ȯ���ִ� ���
        //ex) Debug.Log(Mathf.Log10(100))

        //�̹�  ���������� ����� �ͼ��� �ּ� ~ �ִ� ���� �� ������ �α� �Լ��� ���Ǿ����ϴ�.
        //�ּҰ� -80, �ִ밡 0
        //�׷��� ��ġ ���� �� Mathf.Log10(�����̴� ��ġ) * 20);�� ���� ������ �����ϰ�
        //�����̴��� �ּ� ���� 0.01�� ��� -80�� 1�� ��� 0�� ���˴ϴ�.


    }
}
