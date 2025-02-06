using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ItemDataSystem : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField descriptionInputField; //�����Է��ʵ�
    public Button savebutton;
    public Button loadbutton;
    public Button deletebutton;
    public bool interactable;   
    //public UnityEvent OnDamaged;
      

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //nameInputField.onEndEdit.AddListener(ValueChange);
        //�� ������� ����� ����� ����Ƽ �ν����Ϳ��� ������ �ʽ��ϴ�.

        //loadbutton.interactable = interactable;
        //��ư�� interactable�� ����ڿ��� ��ȣ�ۿ� ���θ� ������ �� ����ϴ� ���Դϴ�. 
    }    
    //1. public���� ���� �Լ��� ����Ƽ �ν����Ϳ��� ���� �����ϰڽ��ϴ�.
    //2. public�� �ƴ� �Լ��� ��ũ��Ʈ �ڵ带 ���� ����� �������ְڽ��ϴ�.
    //3. 

    public void sample()
    {
        Debug.Log("INPUT FIELD's ON VALUE CHANGED");
    }

    /// <summary>
    /// �۾��� �������Ǿ��� �� �ش� ������ �Է������� �˷��ִ� �Լ�
    /// </summary>
    /// <param name="text">����</param>
    //void ValueChange(string text)
    //{
    //    Debug.Log($"{text} �Է� �߽��ϴ�.");
    //}

    public void Save()
    {
        if (nameInputField.text != "" && descriptionInputField.text != "")
        {
            PlayerPrefs.SetString("ItemName", nameInputField.text);
            PlayerPrefs.SetString("Description", descriptionInputField.text);
            Debug.Log("�����Ͱ� ����Ǿ����ϴ�.");
        }
    }

    public void Load()
    {
        if ( PlayerPrefs.HasKey("ItemName") && PlayerPrefs.HasKey("Description"))
        {
            nameInputField.text = PlayerPrefs.GetString("ItemName");
            descriptionInputField.text = PlayerPrefs.GetString("Description");
            Debug.Log("�����Ͱ� �ҷ��Խ��ϴ�.");
        }     
        else
        {
            loadbutton.interactable = interactable;
            Debug.Log("�����Ͱ� �����ϴ�.");
        }
       
    }

    public void Delete()
    {        
        PlayerPrefs.DeleteAll(); //��ü����
        Debug.Log("�����͸� �����ϰڽ��ϴ�.");
    }
}
