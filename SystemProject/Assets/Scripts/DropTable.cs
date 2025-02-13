using System.Collections.Generic;
using UnityEngine;

//CreateAssetMenu�� ����
//()�ȿ� fileName, menuName, order�� ������ �� �ֽ��ϴ�.
//fileName: �����Ǵ� ������ �̸�
//menuName: Create�� ���� ��������� �޴��� �̸��� �����մϴ�. /�� ���� ��� ��ΰ� �߰��˴ϴ�.
//order: �޴� �߿��� ���° ��ġ�� ������ �� ǥ���� �� �����ϴ� ��, ���� Ŭ���� �������� ǥ��˴ϴ�.

[CreateAssetMenu(fileName = "DropTable", menuName = "DropTable/drop table", order = 0)]

public class DropTable : ScriptableObject
{
    public List<GameObject> drop_table;

}
