using UnityEngine;

public enum ItemType
{
    arrow, key, life
}

public class ItemData : MonoBehaviour
{
    public ItemType type;
    public int count = 1;
    public int arrageId = 0; //�ĺ� ��

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Ÿ�Կ� ���� ó���� ����
            switch(type)
            {
                case ItemType.arrow:
                    ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                    ItemKeeper.hasArrows += count;
                    break;
                case ItemType.key:
                    ItemKeeper.hasKeys++;
                    break;
                case ItemType.life:
                    if(PlayerController.hp < 3)
                    {
                        PlayerController.hp++;
                    }
                    break;
            }
            //[������ ȹ�� ���� ����]
            //1. �������� ���������� ������ �ִ� �ݶ��̴��� ��Ȱ��ȭ�մϴ�.
            GetComponent<CircleCollider2D>().enabled = false;
            //2. �������� ���������� ������ �ִ� ������ٵ� ���� ���� Ƣ������� �������̤� ǥ���մϴ�.
            var item_rbody = GetComponent<Rigidbody2D>();
            item_rbody.gravityScale = 2.5f;
            item_rbody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);


        }
    }

}
