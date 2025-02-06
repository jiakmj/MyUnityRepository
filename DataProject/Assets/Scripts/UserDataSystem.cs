using UnityEngine;

public class UserDataSystem : MonoBehaviour
{
    public UserData data01;
    public UserData data02;


    //PlayerPrefs ���
    //1. DeleteAll() ���� ���
    //2. DeleteKey(Ű �̸�) �ش� Ű�� �ش��ϴ� ���� �����մϴ�.
    //3. GetFloat/Int/String(Ű �̸�) Ű�� �ش��ϴ� ���� return �մϴ�.
    //                                ������ Ÿ�Կ� ���缭 ����մϴ�.
    //4. SetFloat/Int/String(Ű �̸�, ��) �ش� Ű - ���� �����մϴ�.
    //                                              ������ ���� Ű�� �ִٸ� ���� ����˴ϴ�.
    //5. HasKey(Ű �̸�) �ش� Ű�� �����ϴ����� Ȯ���մϴ�.

    private void Start()
    {
        data01 = new UserData();
        //Ŭ���� ���� ���
        //Ŭ��������(���۷���)�� = new ������();

        data02 = new UserData("sample0206", "choi", "asd123", "choi0210@naver.com");

        //data02�� �����͸� ���̵�, �̸�, ��й�ȣ, �̸��� ������ ����
        string data_value = data02.GetData();
        Debug.Log(data_value);

        PlayerPrefs.SetString("Data01", data_value); //�� �����͸� Data01�� ����
        PlayerPrefs.Save();

        data01 = UserData.SetData(data_value); //data01�� ���޹��� �����ͷ� ����

        Debug.Log(data01.GetData()); //data01�� ����� ���޵Ǿ����� Ȯ��

    }   
    


}
