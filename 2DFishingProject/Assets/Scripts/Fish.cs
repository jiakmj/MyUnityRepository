using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody2D rigid;
    public int hp;

    /// <summary>
    /// 물고기 생성 시 일정한 힘과 랜덤으로 물고기가 잡힘
    /// </summary>
    private void OnEnable()
    {
        rigid.AddForce(new Vector2(4.3f, 7.5f), ForceMode2D.Impulse);
        int torqueX = Random.Range(1, 6);
        int torqueY = Random.Range(1, 6);
        rigid.AddForce(new Vector2(torqueX, torqueY), ForceMode2D.Impulse);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
