using System;
using UnityEngine;

public enum QuestType
{
    normal, daily, weekly
}

//�Ϲ� ����Ʈ: Ŭ�����ϸ� �� �̻� �� �� �����ϴ�.
//���ϸ� ����Ʈ: ������ �������� ����Ʈ�� ���ŵ˴ϴ�.
//��Ŭ�� �׽�Ʈ: �ָ� ������������Ʈ�� ���ŵ˴ϴ�.

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/quest")]

public class Quest : ScriptableObject
{
    public QuestType ����Ʈ����;
    public Reward ����;
    public Requirement �䱸����;

    [Header("����Ʈ ����")]
    public string ����; // ����Ʈ�� ����
    public string ��ǥ; // ����Ʈ�� ��ǥ
    [TextArea] public string ����; //����Ʈ�� ���� ���

    public bool ����; // ����Ʈ�� ���� ���θ� üũ�մϴ�.
    public bool �������; //����Ʈ�� ���� �������� Ȯ���ϴ� �뵵�� ����մϴ�.

}

//�䱸 ���ǿ� ���� ��ũ���ͺ� ������Ʈ ����
[Serializable]
[CreateAssetMenu(fileName = "Requirement", menuName = "Quest/Requirement")]
public class Requirement : ScriptableObject
{
    public int ��ǥ���ͼ�;
    public int �����������ͼ�;


}


[Serializable]
[CreateAssetMenu(fileName = "Reward", menuName = "Quest/Reward")]
public class Reward : ScriptableObject
{
    public int ��;
    public int ����ġ;

    


}
