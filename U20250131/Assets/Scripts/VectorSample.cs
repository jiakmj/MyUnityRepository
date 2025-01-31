using UnityEngine;

public class VectorSample : MonoBehaviour
{
    //�⺻ ���� (x, y, z)������ ���� �ۼ�
    //0, 0, 0
    Vector3 vec = new Vector3();
    float x, y, z;

    //Vector3 custom_vec = new Vector3(x, y, z);

    //����Ƽ �⺻ ����(���� ��)
    //ex) Vector3 a = Vector3.up;
    //�̸� ��ǥ
    //up(0, 1, 0) down(0, -1, 0)
    //left(-1, 0, 0) right(1, 0, ,)
    //forward(0, 0, 1) back(0, 0, -1)`
    //one(1, 1, 1) zero(0, 0, 0)

    //���� �⺻ ����(����, ����, ������, ����)
    Vector3 a = new Vector3(1, 2, 0);
    Vector3 b = new Vector3(3, 4, 0);

    Vector3 some = Vector3.zero;
    float point = 5.0f;

    //���� ����

    Vector3 Asite = new Vector3(10, 0, 0);
    Vector3 Bsite = new Vector3(5, 0, 0);

    float attck_position = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //����: �ܰ������� �ϳ��� ���ʴ�� ó���Ѵ�.
        //������ ����Ǿ ����� �����ϴ�.
        //Ư�� �����ǿ��� ������ ������ ��� ó��(��ġ �̵�, ��Ż)
        Vector3 c = a + b;
        var trap_air = some + new Vector3(0, point);
        //var�� C#���� ���� �������� �����͸� �ڵ����� �����ϴ� Ű�����Դϴ�.
        //���� �ڵ� �������δ� ���� ���� Vector3�̱� ������ var�� Vector3

        //����
        //�� ������Ʈ���� �ٸ� ������Ʈ������ �Ÿ��� ������ ���ϴ� ��Ȳ�� ����մϴ�.
        //������ ���� ������ �߿��մϴ�.
        Vector3 d = a - b;

        Vector3 distance = Asite - Bsite;
        //�� �Ÿ��� ���� �� ������ �Ÿ��� ���ų� �����ٸ� �����ض� ���� �ڵ带 ¥�� �����ϴ�.

        //����
        //������ �� ���п� ��Į�� ���� ���Ѵ�.
        //���� * ��Į�� => ������ ������ ������ ����Ű�� ����
        //���� ��ü���� ���� �� �ְ� ������ ũ�⸦ �����ϴ� ��쿡 ����մϴ�.
        Vector3 e = a * 2;

        //������
        Vector3 Position = Vector3.one; //(1, 1, 1)
        Position = Position * 4; //(4, 4, 4)
        Position = Position / 4; //(1, 1, 1)

        //������ ����
        //����: 2D, 3D �� ����
        // >> �� ������ ������ ���ϰ� �� ����� ��� ���ϴ� ���� ���
        Vector3 k = new Vector3(1, 2, 3);
        Vector3 l = new Vector3(4, 5, 6);

        float dot = Vector3.Dot(k, l);
        //k * l = (kx * lx) + (ky * ly) + (kz * lz);
        //4 + 10 + 18 = 32
        //�� ����� �� ��ǥ�� �󸶳� ���������� �Ǵ��մϴ�.
        // >> �� ���� ������ ����

        //����: 3D���� ���(3D �׷���)
        //���� ���� ��� �ÿ� ���˴ϴ�. (������ ����̳� ������ ���Ͽ� ������ ���� �ǹ�)
        Vector3 cross = Vector3.Cross(k, l);
        //k * l = (ky * lz - kz  * ly, kz * lx * kx * lz, kx * ky = ky * kx)

        //������ ũ��(������ ����)
        Vector3 m = new Vector3(1, 2, 3);
        float mag = m.magnitude;
        //������ �� ������ ���� ���� ������



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
