using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    public Text message; //Ÿ���� �ؽ�Ʈ
    [SerializeField][TextArea]
    private string content; //����� ����

    [SerializeField]
    private float delay = 0.2f; //�д� �ӵ�



    void Start()
    {
        
    }

    public void onMessageButtonClick()
    {
        StartCoroutine("Typing");
    }

    /// <summary>
    /// 2��� ���
    /// </summary>
    public void ByTwo()
    {
        if(delay == 0.2f)
        
            delay = 0.1f;
        
        else
        
            delay = 0.2f;
        
    }


    IEnumerator Typing()
    {
        message.text = ""; //���� ȭ���� �޼����� ����ڽ��ϴ�.

        int typing_count = 0; //Ÿ���� ī��Ʈ�� -���� �����մϴ�.


        //���� ī��Ʈ�� �������� ���̿� �ٸ��ٸ� 
        while(typing_count != content.Length)
        {
            if (typing_count < content.Length)
            {
                message.text += content[typing_count].ToString();
                //���� ī��Ʈ�� �ش��ϴ� �ܾ� �ϳ��� �޼��� �ؽ�Ʈ UI�� �����մϴ�.
                typing_count++;
                //ī��Ʈ�� 1 ������Ų��.

            }

            yield return new WaitForSeconds(delay);
            //������ �����̸�ŭ ����մϴ�.


        }


    }


}
