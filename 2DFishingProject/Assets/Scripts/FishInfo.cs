using UnityEngine;
using UnityEngine.UI;

public class FishInfo : MonoBehaviour
{
    public GameObject fishInfoPanel;
    public Text fishNameText; //물고기 이름
    public Text fishLevelText; //물고기 난이도
    public Image fishImage; //물고기 이미지

    public Button fishCheckButton;

    void Start()
    {
        fishInfoPanel.SetActive(false);
        fishCheckButton.onClick.AddListener(HideFishInfo);
    }

    public void ShowFishInfo(Fish fish)
    {
        fishInfoPanel.SetActive(true);
        fishNameText.text = $"이름: {fish.name}";
        fishLevelText.text = $"난이도: {fish.level}";
        fishImage.sprite = fish.sprite; //물고기 이미지 표시
    }

    public void HideFishInfo()
    {
        fishInfoPanel.SetActive(false);
    }    
}
