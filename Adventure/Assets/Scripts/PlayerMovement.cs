using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float runSpeed = 7.0f;
    public float jumpForce = 10.0f;

    private Rigidbody2D rb;
    private bool isGrounded;
    //private bool isRunning;

    [Header("Ground Check")] //땅에 닿았는지 안 닿았는지 확인 레이캐스트로 해도 괜찮음
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private PlayerAnimation playerAnimation;
    private PlayerAttach playerAttach;

    public GameObject bowObject;   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();      
        playerAttach = GetComponent<PlayerAttach>();
    }

    public void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); //좌우 이동
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        //addForce와 Velocity차이
        //addForce는 순간적인 힘 Velocity는 일정한 힘으로 밀어냄

        if (playerAnimation != null)
        {
            playerAnimation.SetWalking(moveInput != 0);
        }

        if (moveInput != 0) 
        {
            bool isFlipped = moveInput < 0;
            GetComponent<SpriteRenderer>().flipX = isFlipped; //좌우반전
            //Debug.Log("이동 입력 : " + moveInput);
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

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //해당 위치에서 원형을 만듦 그래서 새 오브젝트를 자식으로 만들어서 위치값을 참고함
        if (Input.GetButtonDown("Jump") && isGrounded && !playerAttach.IsHoldingObject()) //점프애니메이션
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            //playerAnimation.SetJumping(true);
            playerAnimation.TriggerJump();
            Debug.Log("점프");
        }
        
        //else if (!isGrounded && rb.linearVelocity.y < -0.1f) //낙하상태
        //{
        //    playerAnimation?.SetFalling(true);
        //}
        //else if(!isGrounded) //착지상태
        //{
        //    playerAnimation?.PlayLanding();
        //}
    }
    public bool IsMoving()
    {
        return Mathf.Abs(rb.linearVelocity.x) > 0.01f;
    }
}
