using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool is_countdown = true; //true: ī��Ʈ �ٿ� ����, false = ī��Ʈ ó��X
    public float game_time = 0;      //���� ������ ���� �ð�(�ִ� �ð�)
    public bool is_timeover = false; //false: Ÿ�̸� �۵� ��, true = Ÿ�̸� ����
    public float display_time = 0;   //ȭ�鿡 ǥ���ϱ� ���� �ð�
    float times = 0;                 //���� �ð�
     
    void Start()
    {
        //ī��Ʈ �ٿ��� ���� ���̶��, ǥ��� �ð��� ���� �ð����� �����մϴ�.
        if(is_countdown)
        {
            display_time = game_time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(is_timeover == false)
        {
            times += Time.deltaTime;

            if (is_countdown)
            {
                display_time = game_time - times;

                if(display_time <= 0.0f)
                {
                    display_time = 0.0f;
                    is_timeover = true;
                }
            }
            else
            {
                display_time = times;

                if(display_time >= game_time)
                {
                    display_time = game_time;
                    is_timeover = true;
                }
            }
        }
        
    }
}
