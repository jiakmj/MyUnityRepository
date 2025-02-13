//using System;
using UnityEngine;

//using System�� ����ϸ鼭 ����Ƽ�� ������ ����ϰ� ���� ���
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    //������ ��� ���̺�
    public DropTable DropTable;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Dead();
        }        
    }
    private void Dead()
    {
        GameObject dropItemPrefab = DropTable.drop_table[Random.Range(0, DropTable.drop_table.Count)];
        //Random.Range(�ּ�, �ִ�)�� ����Ƽ���� �������ִ� ���� ����
        //�ּ� ������ �ִ� -1���� ������ �� �� �ϳ��� �������� �����մϴ�.

        Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
