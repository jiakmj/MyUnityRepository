using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine; //NameSpace: �Ҽ�

public class PlayerManager : MonoBehaviour
{
    private float moveSpeed = 5.0f; //�÷��̾� �̵� �ӵ�
    public float mouseSensitivity = 100.0f; //���콺 ����
    public Transform cameraTransform; //ī�޶��� Transform
    public CharacterController characterController;
    public Transform playerHead; //�÷��̾� �Ӹ� ��ġ(1��Ī ��带 ���ؼ�)
    public float thirdPersonDistance = 3.0f; //3��Ī ��忡�� �÷��̾�� ī�޶��� �Ÿ�
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f); //3��Ī ��忡�� ī�޶� ������
    public Transform playerLookObj; //�÷��̾� �þ� ��ġ

    public float zoomDistance = 1.0f; //ī�޶� Ȯ��� ���� �Ÿ�(3��Ī ��忡�� ���)
    public float ZoomSpeed = 5.0f; //Ȯ����Ұ� �Ǵ� �ӵ�
    public float defaultFov = 60.0f; //�⺻ ī�޶� �þ߰�
    public float zoomFov = 30.0f; //Ȯ�� �� ī�޶� �þ߰�(1��Ī ��忡�� ���)

    private float currentDistance; //���� ī�޶���� �Ÿ�(3��Ī ���)
    private float targetDistance; //��ǥ ī�޶� �Ÿ�
    private float targetFov; //��ǥ FOV
    private bool isZoomed = false; //Ȯ�� ���� Ȯ��
    private Coroutine zoomCoroutine; //�ڷ�ƾ�� ����Ͽ� Ȯ�� ��� ó��
    private Camera mainCamera; //ī�޶� ������Ʈ

    private float pitch = 0.0f; //���Ʒ� ȸ�� ��
    private float yaw = 0.0f; //�¿� ȸ�� ��
    private bool isFirstPerson = false; //1��Ī ��� ����
    private bool isRotaterAroundPlayer = true; //ī�޶� �÷��̾� ������ ȸ���ϴ��� ����

    //�߷� ���� ����
    public float gravity = -9.81f; //Character Controller���� gravity�� ���� //����, �浹�� ���� �κ��� ������ ������ �߷�, ������ ����
    public float jumpHeight = 2.0f; //���� ����
    private Vector3 velocity;
    private bool isGround;

    private Animator animator;
    private float horizontal;
    private float vertical;
    private bool isRunning = false;
    public float walkSpeed = 5.0f; //�ȴ� �ӵ�
    public float runSpeed = 10.0f; //�ٴ� �ӵ�   
    private bool isAim = false;
    private bool isFire = false;
    private bool isPickUp = false;

    public AudioClip audioClipFire;
    private AudioSource audioSource;
    public AudioClip audioClipWeaponChange;
    public AudioClip audioClipPickUp;
    public GameObject SMGObj;
    private int animationSpeed = 1; //�ִϸ��̼� �ӵ� ����
    private string currentAnimation = "Idle";

    public Transform aimTarget;

    private float weaponMaxDistance = 100.0f;
    

    void Start()
    {             
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = thirdPersonDistance;
        targetDistance = thirdPersonDistance;
        targetFov = defaultFov;
        mainCamera = cameraTransform.GetComponent<Camera>();
        mainCamera.fieldOfView = defaultFov; //����, �ܾƿ��� �ϱ� ���ؼ�
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SMGObj.SetActive(false);
    }

    void Update()
    {
        MouseSet();

        CameraSet();

        PlayerMovement();

        PlayerRun();

        AimSet();

        Fire();

        ChangeTools();

        AnimationSet();

        PickUp();

        //1�� ���
        //animator.speed = animationSpeed;

        //2�� ���
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(stateInfo.IsName("HitReaction"));
        //if (stateInfo.IsName(currentAnimation))
        //{
        //    Debug.Log("stateInfo.normalizedTime : " + stateInfo.normalizedTime);
        //}

        if (stateInfo.IsName("HitReaction") && stateInfo.normalizedTime >= 1.0f) //�ִϸ��̼��� �� ���̸� 1�� ���� 1.0�� ������ �� �ִϸ��̼��� ������ �ǹ��� 
        {
            //�� �ִϸ��̼��� ������ �ٸ� �ִϸ��̼��� ���� ������
            currentAnimation = "Attack";
            animator.Play(currentAnimation);

        }
    }

    void UpdateAimTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ī�޶��� ���鿡�� ���콺 �����͸� �������� ������ �߻��ϴ°�
        aimTarget.position = ray.GetPoint(10.0f); //�Ÿ� ������

    }

    void PickUp()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.E) && !stateInfo.IsName("PickUp"))
        {
            animator.SetTrigger("PickUp"); 
        }
    }

    void MouseSet()
    {
        //���콺 �Է��� �޾� ī�޶�� �÷��̾� ȸ�� ó��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -45f, 45f); //3��Ī �������� (1��Ī�� �ٸ�)

    }

    void CameraSet()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            Debug.Log(isFirstPerson ? "1��Ī ���" : "3��Ī ���");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isRotaterAroundPlayer = !isRotaterAroundPlayer;
            Debug.Log(isRotaterAroundPlayer ? "ī�޶� ������ ȸ���մϴ�." : "�÷��̾��� �þ߿� ���� ȸ���մϴ�");
        }

        if (isFirstPerson)
        {
            FirstPersonMovement();
        }
        else
        {
            ThirdPersonMovemnet();
        }
    }

    void PlayerMovement()
    {
        isGround = characterController.isGrounded;

        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
            //���� �پ��ְ� �߶��ϰ� ���� �� Ȥ�� �� ��Ȳ�� ���� ����
        }
    }

    void PlayerRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        moveSpeed = isRunning ? runSpeed : walkSpeed; //isRunning�� true�� runSpeed, false�� walkSpeed�� moveSpeed�� ����  
    }

    void AimSet()
    {
        if (Input.GetMouseButtonDown(1)) //��Ŭ����
        {
            

            isAim = true;
            //animator.SetBool("isAim", isAim);
            animator.SetLayerWeight(1, 1);

            if (zoomCoroutine != null) //�ߺ�����, �ڷ�ƾ�� �۵� ������ �ƴ��� Ȯ�� �� ����
            {
                StopCoroutine(zoomCoroutine);
            }

            if (isFirstPerson) //1��Ī�̶��(ĳ���� �Ӹ��� �޸�) ī�޶� Ȯ����
            {
                SetTargetFOV(zoomFov); //�þ߰� �̵� Ȯ����
                zoomCoroutine = StartCoroutine(ZoomFieldOfView(targetFov));
            }
            else //3��Ī�̶�� ī�޶� ��ġ �̵�, ��� ���� ����...?
            {
                SetTargetDistance(zoomDistance); //Ÿ�� �Ÿ� ����
                zoomCoroutine = StartCoroutine(ZoomCamera(targetDistance));
            }
        }

        if (Input.GetMouseButtonUp(1)) //���� �� 
        {
            isAim = false;
            //animator.SetBool("isAim", isAim);
            animator.SetLayerWeight(1, 0);

            if (zoomCoroutine != null) //�ߺ�����
            {
                StopCoroutine(zoomCoroutine);
            }

            if (isFirstPerson) //1��Ī
            {
                SetTargetFOV(defaultFov); //�⺻ �������� ���ư�
                zoomCoroutine = StartCoroutine(ZoomFieldOfView(targetFov));
            }
            else //3��Ī
            {
                SetTargetDistance(thirdPersonDistance); //����� ��
                zoomCoroutine = StartCoroutine(ZoomCamera(targetDistance));
            }
        }
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Weapon Type MaxDistance Set ���⿡ ���� �ִ� �����Ÿ� �����ؾ� ��
            weaponMaxDistance = 1000.0f;

            if (isAim == true)
            {
                isFire = true;
                animator.SetTrigger("Fire");

                Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, weaponMaxDistance))
                {
                    Debug.Log("Hit : " + hit.collider.gameObject.name);
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 2.0f);
                }
                else
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * weaponMaxDistance, Color.green, 2.0f);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isFire = false;            
        }
    }

    void ChangeTools()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1�� ������ �ֹ��� ����
        {            
            animator.SetTrigger("isWeaponChange");
            SMGObj.SetActive(true);
        }
    }  

    void AnimationSet()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetBool("isRunning", isRunning);        
    }

    void FirstPersonMovement() //1��Ī �̵�
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal; //ī�޶� �����̵� �÷��̾� �����̵��� ���Ͻ�
        moveDirection.y = 0; //��鸲 ����, ������ ���װ� ����
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime); //�̵�

        cameraTransform.position = playerHead.position; //ī�޶� ������ �÷��̾� ������ ���°�ó�� ��ġ ����
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0);

        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0);
    }

    void ThirdPersonMovemnet() //3��Ī �̵�
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        if (isRotaterAroundPlayer)
        {
            //ī�޶� �÷��̾� �����ʿ��� ȸ���ϵ��� ����
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

            //ī�޶� �÷��̾��� �����ʿ��� ������ ��ġ�� �̵�
            cameraTransform.position = transform.position + thirdPersonOffset + rotation * direction;

            //ī�޶� �÷��̾��� ��ġ�� ���󰡵��� ����
            cameraTransform.LookAt(transform.position + new Vector3(0, thirdPersonOffset.y, 0)); //transform�� �÷��̾��̱� ������ ���� ����!
        }
        else
        {
            //�÷��̾ ���� ȸ���ϴ� ���
            transform.rotation = Quaternion.Euler(0f, yaw, 0);
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            cameraTransform.position = playerLookObj.position + thirdPersonOffset + Quaternion.Euler(pitch, yaw, 0) * direction;
            cameraTransform.LookAt(playerLookObj.position + new Vector3(0, thirdPersonOffset.y, 0)); //ī�޶� ĳ���͸� ������� ���� �� �ǰ� ���� ���� �־�� �ؼ�

            UpdateAimTarget();
        }
    }

    public void SetTargetDistance(float distance) //Ÿ�� �Ÿ��� �����ϴ� �Լ�
    {
        targetDistance = distance;
    }

    public void SetTargetFOV(float fov) 
    {
        targetFov = fov;
    }

    /// <summary>
    /// 3��Ī �ܿ� ���
    /// </summary>
    /// <param name="targetDistance"></param>
    /// <returns></returns>
    IEnumerator ZoomCamera(float targetDistance)
    {
        while (Mathf.Abs(currentDistance - targetDistance) > 0.01f) //���� �Ÿ����� ��ǥ �Ÿ��� �ε巴�� �̵�
            {
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * ZoomSpeed);
            yield return null;
        }
        currentDistance = targetDistance; //��ǥ �Ÿ��� ������ �� ���� ����
    }

    /// <summary>
    /// 1��Ī �ܿ� ���
    /// </summary>
    /// <param name="targetFov"></param>
    /// <returns></returns>
    IEnumerator ZoomFieldOfView(float targetFov) 
    {
        while(Mathf.Abs(mainCamera.fieldOfView - targetFov) > 0.01f)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFov, Time.deltaTime * ZoomSpeed);
            yield return null;
        }
        mainCamera.fieldOfView = targetFov;
    }  
    
    public void WeaponChangeSoundOn()
    {
        audioSource.PlayOneShot(audioClipWeaponChange);
    }

    public void FireSoundOn()
    {
        audioSource.PlayOneShot(audioClipFire); 
    }

    public void PickUpSoundOn()
    {
        audioSource.PlayOneShot(audioClipPickUp);
    }

    //public void MovementSoundOn()
    //{
    //    if ()
    //    {

    //    }
    //    audioSource.PlayOneShot();

    //}

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("PlayerDamage"))
        {
            //animationSpeed = 2; //1�� ���
            FireSoundOn();
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("Damage");
            characterController.enabled = false;
            gameObject.transform.position = Vector3.zero;     
            characterController.enabled = true;
        }        
    }
}
