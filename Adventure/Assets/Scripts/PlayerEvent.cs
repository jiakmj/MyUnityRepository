using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public GameObject ArrowKeyObj;
    public GameObject SpaceKeyObj;
    public GameObject Mouse1KeyObj;
    public GameObject Mouse2HoldObj;
    public GameObject BowKeyObj;
    public GameObject FoodObj;

    //[Header("GameScene1")]
    //public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "TutorialEvent1")
        {
            ArrowKeyObj.SetActive(true);
            //�Ҹ� ������ ����
        }
        else if ( collision.name == "TutorialEvent2")
        {
            SpaceKeyObj.SetActive(true);
        }
        else if (collision.name == "TutorialEvent3")
        {
            Mouse1KeyObj.SetActive(true);
        }
        else if (collision.name == "TutorialEvent4")
        {
            Mouse2HoldObj.SetActive(true);
        }
        else if (collision.name == "TutorialEvent5")
        {
            BowKeyObj.SetActive(true);
        }
        else if (collision.name == "TutorialEvent6")
        {
            FoodObj.SetActive(true);
        }
        //else if (collision.CompareTag("Finish"))
        //{
        //    Debug.Log("Ʃ�丮�� ���� ���������� �̵�: " + nextSceneName);
        //    SceneController.Instance.StartSceneTransition("GameScene1");
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "TutorialEvent1")
        {
            ArrowKeyObj.SetActive(false);
        }
        else if ( collision.name == "TutorialEvent2")
        {
            SpaceKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent3")
        {
            Mouse1KeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent4")
        {
            Mouse2HoldObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent5")
        {
            BowKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent6")
        {
            FoodObj.SetActive(false);
        }
    }
}
