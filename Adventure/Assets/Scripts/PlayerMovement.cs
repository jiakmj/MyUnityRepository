using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float runSpeed = 7.0f;
    public float jumpForce = 10.0f;

    private Rigidbody2D rb;
    private bool isGrounded;
    //private bool isRunning;

    [Header("Ground Check")] //���� ��Ҵ��� �� ��Ҵ��� Ȯ�� ����ĳ��Ʈ�� �ص� ������
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private PlayerAnimation playerAnimation;

    public GameObject bowObject;   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); //�¿� �̵�
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        //addForce�� Velocity����
        //addForce�� �������� �� Velocity�� ������ ������ �о

        if (playerAnimation != null)
        {
            playerAnimation.SetWalking(moveInput != 0);
        }

        if (moveInput != 0) 
        {
            bool isFlipped = moveInput < 0;
            GetComponent<SpriteRenderer>().flipX = isFlipped; //�¿����
            //Debug.Log("�̵� �Է� : " + moveInput);
            {
                if (bowObject != null)
                {
                    Vector3 scale = bowObject.transform.localScale;
                    scale.x = Mathf.Abs(scale.x) * (isFlipped ? -1 : 1);
                    bowObject.transform.localScale = scale;

                    Vector3 pos = bowObject.transform.localPosition;
                    pos.x = isFlipped ? -0.29f : 0.259f;
                    bowObject.transform.localPosition = pos;
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimation.SetRunning(true);
        }
        else
        {
            playerAnimation.SetRunning(false);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //�ش� ��ġ���� ������ ���� �׷��� �� ������Ʈ�� �ڽ����� ���� ��ġ���� ������
        if (Input.GetButtonDown("Jump") && isGrounded) //�����ִϸ��̼�
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            //playerAnimation.SetJumping(true);
            playerAnimation.TriggerJump();
            Debug.Log("����");
        }
        
        //else if (!isGrounded && rb.linearVelocity.y < -0.1f) //���ϻ���
        //{
        //    playerAnimation?.SetFalling(true);
        //}
        //else if(!isGrounded) //��������
        //{
        //    playerAnimation?.PlayLanding();
        //}
    }
}
