using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject main_image;
    public Sprite game_over_sprite;
    public Sprite game_clear_sprite;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;

    Image image;

    //---Time Controller---
    public GameObject time_bar;
    public GameObject timeText;
    TimeController timeController;

    void Start()
    {
        //Ÿ�� ��Ʈ�ѷ� ���� �� ����
        timeController = GetComponent<TimeController>();

        if(timeController != null )
        {
            if(timeController.game_time == 0.0f)
            {
                time_bar.SetActive(false); //�ð� ������ ���ٸ� ����ڽ��ϴ�.
            }
        }
        //���� �ؽ�Ʈ�� �гο� ���� ����

        //1�� �ڿ� �ش� �Լ��� ȣ���մϴ�.
        Invoke("InactiveImage", 1.0f);
        //�г� ��Ȱ��ȭ
        panel.SetActive(false);
    }

    void InactiveImage()
    {
        main_image.SetActive(false);
    }

    void Update()
    {
        if (PlayerController.state == "gameclear")
        {
            //�̹��� Ȱ��ȭ
            main_image.SetActive(true);
            //�г� Ȱ��ȭ
            panel.SetActive(true);

            //�ٽ� ���� ��ư�� ���� ��Ȱ��ȭ(������ Ŭ���������ϱ�)
            restartButton.GetComponent<Button>().interactable = false;
            //���� �̹����� ���� Ŭ���� �̹����� �����մϴ�.
            main_image.GetComponent<Image>().sprite = game_clear_sprite;
            //�÷��̾� ��Ʈ�ѷ��� ���¸� end�� �����մϴ�.
            PlayerController.state = "end";

            if(timeController != null)
            {
                timeController.is_timeover = true;
            }

        }
        else if(PlayerController.state == "gameover")
        {
            //�̹��� Ȱ��ȭ
            main_image.SetActive(true);
            //�г� Ȱ��ȭ
            panel.SetActive(true);

            //���� ��ư�� ���� ��Ȱ��ȭ(������ Ŭ�������� ���߱� ����)
            nextButton.GetComponent<Button>().interactable = false;
            //���� �̹����� ���� ���� �̹����� �����մϴ�.
            main_image.GetComponent<Image>().sprite = game_over_sprite;
            //�÷��̾� ��Ʈ�ѷ��� ���¸� end�� �����մϴ�.
            PlayerController.state = "end";

            if (timeController != null)
            {
                timeController.is_timeover = true;
            }
        }
        else if(PlayerController.state == "playing")
        {
            //���� ���� �ÿ� ���� �߰� ó���� �����ϴ� �ڸ�

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            PlayerController playerController = player.GetComponent<PlayerController>();

            if(timeController != null)
            {
                if(timeController.game_time > 0.0f)
                {
                    //ǥ��� ������ ���̰� ����(�Ҽ��� �����ϴ�.)
                    int time = (int)timeController.display_time;
                    //�ð� ����(UI)
                    timeText.GetComponent<Text>().text = time.ToString();

                    if(time == 0)
                    {
                        playerController.GameOver();
                    }
                }
            }

        }
    }
}
