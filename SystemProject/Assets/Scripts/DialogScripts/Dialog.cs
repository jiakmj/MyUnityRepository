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

        squeue.Enqueue("누군가 도와주세요.");
        squeue.Enqueue("무슨 일이죠?");
        squeue.Enqueue("바나나 껍질 20개가 필요해요.");
        squeue.Enqueue("알겠습니다. 도와드리겠습니다.");
        squeue.Enqueue("감사합니다.");

        //string sampleText = "안녕하세요 샘플입니다.";
        //StartCoroutine(Typing(sampleText));
        StartCoroutine(DisplayQueuecontent());
              
    }
    
    void Update()
    {
        if (squeue.Count == 0 && !isDialogFinished)
        {
            isDialogFinished = true;
            text.text = "";
            Debug.Log("퀘스트를 받았다.");
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

