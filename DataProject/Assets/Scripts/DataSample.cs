using UnityEngine;

public class DataSample : MonoBehaviour
{
    //1. ����Ƽ �������� ����
    //������ ���� �������� ���� �ٽ��� �Ǵ� �κ�(������)

    //�Ϲ����� �۾� ��, �÷��� �ϴ� ��쿡�� ���� ���� �����ǰ�
    //�ٽ� �÷����ϸ� ������ ��ϵ��� ���ŵ�.

    //���� ���ο� ���� �÷����� �� ������ ������ ������ �����ϴ� ���ӵ� ����

    //PlayerpPrefs�� �ַ� �÷��̾ ���� ȯ�� ������������ �� ���Ǵ� Ŭ�����Դϴ�.
    //�ش� Ŭ������ ���ڿ�, �Ǽ�, ������ ������� �÷��� ������Ʈ���� ������ �� �ֽ��ϴ�.

    //PlayerPrefs�� Key�� Value�� �����Ǿ� �ִ� �������Դϴ�. (C#�� Dictionary)

    //Key�� Value�� �����ϱ� ���� ������(���� �������� ��ġ)
    //Value�� Key�� ���� ������ �� �ִ� �������� ��

    //ex) userID: current147���� ����Ǿ� �ִٸ�, userID�� Key, current147�� Value�� �ش��Ѵٰ� �����մϴ�. 

    public UserData userData;

    //1. ����Ƽ �����Ϳ��� ���� userdata�� ���� ������ �� �ۼ��ϰڽ��ϴ�.
    //2. ������Ʈ���� �ִ� Ű ���� �˻��غ��ڽ��ϴ�.
    //3. Ű ��ü ����

    private void Start()
    {
        /*PlayerPrefs.SetString("ID", userData.UserID);
        PlayerPrefs.SetString("UserName", userData.UserName);
        PlayerPrefs.SetString("Password", userData.UserPassword);
        PlayerPrefs.SetString("E-mail", userData.UserEmail);*/

        //Debug.Log("�����Ͱ� ����Ǿ����ϴ�.");

        //Debug.Log(PlayerPrefs.GetString("ID"));
        //PlayerPrefs.DeleteAll(); //��ü����
        //Debug.Log("�����͸� �����ϰڽ��ϴ�.");



    }


}
