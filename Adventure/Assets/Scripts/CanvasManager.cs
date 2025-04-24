using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Image backgroundImage;  // ��� �̹���
    public GameObject pauseUI;  // ���� ������ ������ UI
    public GameObject hpUI;  // HP UI
    public GameObject itemUI1;
    public GameObject itemUI2;
    public GameObject itemUI3;

    private void Start()
    {
        // ���� ���� �°� backgroundImage ����
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            // ���� �޴����� ��� �̹��� ǥ��
            backgroundImage.gameObject.SetActive(true);
        }
        else
        {
            // ���� �������� ��� �̹��� �����
            backgroundImage.gameObject.SetActive(false);
        }
    }

    public void InitializeUI(bool showHP, bool showItems)
    {
        hpUI.SetActive(showHP);  // ���� ���� �� HP UI Ȱ��ȭ
        itemUI1.SetActive(showItems);
        itemUI2.SetActive(showItems);
        itemUI3.SetActive(showItems);
    }
}
