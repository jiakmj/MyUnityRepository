using UnityEngine;

//Ŭ������ ���� ����ȭ
[System.Serializable]
public class UserData
{
    public string UserID;
    public string UserName;
    public string UserPassword;
    public string UserEmail;

    //������(Constructor)
    //Ŭ������ �̸��� ������ �޼ҵ带 �ǹ��մϴ�.
    //�Լ��� ��ȯ Ÿ���� ���� ���� �޼ҵ��Դϴ�.
    //���� �������� ���� ��쿡�� �⺻ �����ڷ� ó���˴ϴ�.
    //���̵�, �̸�, ��й�ȣ, �̸��� ������� �ۼ��ϸ� ������ �� �ִ� UserData

    //�⺻ ������ = Ŭ������ �̸��� ������ �޼ҵ�, �Ű������� ���� ���� �ʽ��ϴ�.

    public UserData()
    {

    }


    public UserData(string userID, string userName, string userPassword, string userEmail)
    {
        UserID = userID;
        UserName = userName;
        UserPassword = userPassword;
        UserEmail = userEmail;
    }

    /// <summary>
    /// �����͸� �ϳ��� ���ڿ��� return �ϴ� �ڵ�
    /// </summary>
    /// <returns>���̵�, �̸�, ��й�ȣ, �̸��� ������ ó���մϴ�.</returns>
    public string GetData() => $"{UserID},{UserName},{UserPassword},{UserEmail}";
    //1��¥�� return �ڵ带 ���� ��� {} ��� => �� �ۼ��� �����մϴ�.

    /// <summary>
    /// �����Ϳ� ���� ������ ���޹ް� UserData�� return�ϴ� �ڵ�
    /// </summary>
    /// <param name="data">���̵�, �̸�, ��й�ȣ, �̸��� ������ �ۼ��� ������</param>
    /// <returns></returns>
    public static UserData SetData(string data)
    {
        string[] data_values = data.Split(',');
        //���ڿ�.Split(",") �ش� ���ڿ��� ()�ȿ� �־��� ,�� �������� �߶� �迭�� ������ݴϴ�.

        return new UserData(data_values[0], data_values[1], data_values[2], data_values[3]);
    
    }

}
