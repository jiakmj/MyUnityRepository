using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Queue<string> squeue = new Queue<string>();
    public Text text;
    public Text dialogText;
    public Button button;
    [SerializeField] private float delay = 0.1f;

    private bool isDialogFinished = false;

    void Start()
    {       
        Button next = button.GetComponent<Button>();
        next.onClick.AddListener(OnClick);

        squeue.Enqueue("������ �����ּ���.");
        squeue.Enqueue("���� ������?");
        squeue.Enqueue("�ٳ��� ���� 20���� �ʿ��ؿ�.");
        squeue.Enqueue("�˰ڽ��ϴ�. ���͵帮�ڽ��ϴ�.");
        squeue.Enqueue("�����մϴ�.");

        //string sampleText = "�ȳ��ϼ��� �����Դϴ�.";
        //StartCoroutine(Typing(sampleText));
        StartCoroutine(DisplayQueuecontent());
              
    }
    
    void Update()
    {
        if (squeue.Count == 0 && !isDialogFinished)
        {
            isDialogFinished = true;
            text.text = "";
            Debug.Log("����Ʈ�� �޾Ҵ�.");
        }


    }
  

    void OnClick()
    {
        //if (squeue.Count > 0)
        //{
        //    string currentDialog = squeue.Dequeue();
        //    StartCoroutine(Typing(currentDialog));
        //}
        foreach (string dialog in squeue)
        {
            text.text = squeue.Peek();
        }
        squeue.Dequeue();

    }

    IEnumerator Typing(string text)
    {
        dialogText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator DisplayQueuecontent()
    {
        while (squeue.Count > 0)
        {
            string currentText = squeue.Dequeue();
            yield return StartCoroutine(Typing(currentText));
        }
    }

}

