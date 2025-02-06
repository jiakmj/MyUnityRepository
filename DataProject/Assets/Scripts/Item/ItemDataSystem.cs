using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ItemDataSystem : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField descriptionInputField; //설명입력필드
    public Button savebutton;
    public Button loadbutton;
    public Button deletebutton;
    public bool interactable;   
    //public UnityEvent OnDamaged;
      

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //nameInputField.onEndEdit.AddListener(ValueChange);
        //이 방식으로 등록한 기능은 유니티 인스펙터에서 보이지 않습니다.

        //loadbutton.interactable = interactable;
        //버튼의 interactable은 사용자와의 상호작용 여부를 제어할 때 사용하는 값입니다. 
    }    
    //1. public으로 만든 함수는 유니티 인스펙터에서 직접 연결하겠습니다.
    //2. public이 아닌 함수는 스크립트 코드를 통해 기능을 연결해주겠습니다.
    //3. 

    public void sample()
    {
        Debug.Log("INPUT FIELD's ON VALUE CHANGED");
    }

    /// <summary>
    /// 작업이 마무리되었을 때 해당 문구를 입력했음을 알려주는 함수
    /// </summary>
    /// <param name="text">문구</param>
    //void ValueChange(string text)
    //{
    //    Debug.Log($"{text} 입력 했습니다.");
    //}

    public void Save()
    {
        if (nameInputField.text != "" && descriptionInputField.text != "")
        {
            PlayerPrefs.SetString("ItemName", nameInputField.text);
            PlayerPrefs.SetString("Description", descriptionInputField.text);
            Debug.Log("데이터가 저장되었습니다.");
        }
    }

    public void Load()
    {
        if ( PlayerPrefs.HasKey("ItemName") && PlayerPrefs.HasKey("Description"))
        {
            nameInputField.text = PlayerPrefs.GetString("ItemName");
            descriptionInputField.text = PlayerPrefs.GetString("Description");
            Debug.Log("데이터가 불러왔습니다.");
        }     
        else
        {
            loadbutton.interactable = interactable;
            Debug.Log("데이터가 없습니다.");
        }
       
    }

    public void Delete()
    {        
        PlayerPrefs.DeleteAll(); //전체삭제
        Debug.Log("데이터를 삭제하겠습니다.");
    }
}
