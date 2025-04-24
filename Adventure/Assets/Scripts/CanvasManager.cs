using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Image backgroundImage;  // 배경 이미지
    public GameObject pauseUI;  // 게임 씬에서 보여질 UI
    public GameObject hpUI;  // HP UI
    public GameObject itemUI1;
    public GameObject itemUI2;
    public GameObject itemUI3;

    private void Start()
    {
        // 현재 씬에 맞게 backgroundImage 설정
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            // 메인 메뉴에서 배경 이미지 표시
            backgroundImage.gameObject.SetActive(true);
        }
        else
        {
            // 게임 씬에서는 배경 이미지 숨기기
            backgroundImage.gameObject.SetActive(false);
        }
    }

    public void InitializeUI(bool showHP, bool showItems)
    {
        hpUI.SetActive(showHP);  // 게임 시작 시 HP UI 활성화
        itemUI1.SetActive(showItems);
        itemUI2.SetActive(showItems);
        itemUI3.SetActive(showItems);
    }
}
