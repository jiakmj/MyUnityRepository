using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0; //�ĺ��� ��
    //�÷��̾ ���踦 ������ ���� ��� ���踦 1�� �Ҹ��ؼ� �����Ҽ� �ֵ��� ����

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ItemKeeper.hasKeys > 0)
        {
            ItemKeeper.hasKeys--;
            Destroy(gameObject);            
        }
        else
        {
            Debug.Log("���谡 �����ϴ�");
             
        }
    }


}
