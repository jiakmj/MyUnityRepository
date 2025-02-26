using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bulletFactory; //�Ѿ� ������
    public GameObject firePosition; //�� �߻� ��ġ
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //Fire1 = Left Ctrl
        {
            //�Ѿ� ����
            GameObject bullet = Instantiate(bulletFactory);
            //�Ѿ� ��ġ ����
            bullet.transform.position = firePosition.transform.position;
        }
    }
}
