using UnityEngine;

//��� ��ũ�� ����� ����̴ϴ�.
//Ȯ���� ��: ���׸����� offset�� �ǵ�ȴ��� �̹����� �и���.
//�ʿ��� ��: ���׸��� ����, ��ũ�Ѹ� �ӵ�
//��� ���� �� ���ΰ�? ����? �Ʒ���? ������?

public class BackGround : MonoBehaviour
{
    public Material backgroundMaterial;
    public float scrollSpeed = 0.2f;
   

    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = Vector2.up;

        backgroundMaterial.mainTextureOffset += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}
