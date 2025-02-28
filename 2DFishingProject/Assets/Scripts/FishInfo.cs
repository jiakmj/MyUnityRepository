using UnityEngine;
using UnityEngine.UI;

public class FishInfo : MonoBehaviour
{
    public GameObject fishInfoPanel;
    public Text fishNameText; //����� �̸�
    public Text fishLevelText; //����� ���̵�
    public Image fishImage; //����� �̹���

    public Button fishCheckButton;

    void Start()
    {
        fishInfoPanel.SetActive(false);
        fishCheckButton.onClick.AddListener(HideFishInfo);
    }

    public void ShowFishInfo(Fish fish)
    {
        fishInfoPanel.SetActive(true);
        fishNameText.text = $"�̸�: {fish.name}";
        fishLevelText.text = $"���̵�: {fish.level}";
        fishImage.sprite = fish.sprite; //����� �̹��� ǥ��
    }

    public void HideFishInfo()
    {
        fishInfoPanel.SetActive(false);
    }    
}
