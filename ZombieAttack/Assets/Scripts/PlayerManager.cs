using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine; //NameSpace: 소속

public class PlayerManager : MonoBehaviour
{
    private float moveSpeed = 5.0f; //플레이어 이동 속도
    public float mouseSensitivity = 100.0f; //마우스 감도
    public Transform cameraTransform; //카메라의 Transform
    public CharacterController characterController;
    public Transform playerHead; //플레이어 머리 위치(1인칭 모드를 위해서)
    public float thirdPersonDistance = 3.0f; //3인칭 모드에서 플레이어와 카메라의 거리
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f); //3인칭 모드에서 카메라 오프셋
    public Transform playerLookObj; //플레이어 시야 위치

    public float zoomDistance = 1.0f; //카메라가 확대될 때의 거리(3인칭 모드에서 사용)
    public float ZoomSpeed = 5.0f; //확대축소가 되는 속도
    public float defaultFov = 60.0f; //기본 카메라 시야각
    public float zoomFov = 30.0f; //확대 시 카메라 시야각(1인칭 모드에서 사용)

    private float currentDistance; //현재 카메라와의 거리(3인칭 모드)
    private float targetDistance; //목표 카메라 거리
    private float targetFov; //목표 FOV
    private bool isZoomed = false; //확대 여부 확인
    private Coroutine zoomCoroutine; //코루틴을 사용하여 확대 축소 처리
    private Camera mainCamera; //카메라 컴포넌트

    private float pitch = 0.0f; //위아래 회전 값
    private float yaw = 0.0f; //좌우 회전 값
    private bool isFirstPerson = false; //1인칭 모드 여부
    private bool isRotaterAroundPlayer = true; //카메라가 플레이어 주위를 회전하는지 여부

    //중력 관련 변수
    public float gravity = -9.81f; //Character Controller에는 gravity가 없음 //스텝, 충돌에 대한 부분을 가지고 있지만 중력, 마찰이 없음
    public float jumpHeight = 2.0f; //점프 높이
    private Vector3 velocity;
    private bool isGround;

    private Animator animator;
    private float horizontal;
    private float vertical;
    private bool isRunning = false;
    public float walkSpeed = 5.0f; //걷는 속도
    public float runSpeed = 10.0f; //뛰는 속도   
    private bool isAim = false;
    private bool isFire = false;
    private bool isPickUp = false;

    public AudioClip audioClipFire;
    private AudioSource audioSource;
    public AudioClip audioClipWeaponChange;
    public AudioClip audioClipPickUp;
    public GameObject SMGObj;
    private int animationSpeed = 1; //애니메이션 속도 조절
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
        mainCamera.fieldOfView = defaultFov; //줌인, 줌아웃을 하기 위해서
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

        //1번 방법
        //animator.speed = animationSpeed;

        //2번 방법
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(stateInfo.IsName("HitReaction"));
        //if (stateInfo.IsName(currentAnimation))
        //{
        //    Debug.Log("stateInfo.normalizedTime : " + stateInfo.normalizedTime);
        //}

        if (stateInfo.IsName("HitReaction") && stateInfo.normalizedTime >= 1.0f) //애니메이션의 총 길이를 1로 보고 1.0이 지나면 이 애니메이션이 끝남을 의미함 
        {
            //이 애니메이션이 끝나면 다른 애니메이션을 실행 시켜줘
            currentAnimation = "Attack";
            animator.Play(currentAnimation);

        }
    }

    void UpdateAimTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //카메라의 정면에서 마우스 포인터를 중점으로 레이저 발사하는거
        aimTarget.position = ray.GetPoint(10.0f); //거리 정해줌

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
        //마우스 입력을 받아 카메라와 플레이어 회전 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -45f, 45f); //3인칭 각도제한 (1인칭은 다름)

    }

    void CameraSet()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            Debug.Log(isFirstPerson ? "1인칭 모드" : "3인칭 모드");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isRotaterAroundPlayer = !isRotaterAroundPlayer;
            Debug.Log(isRotaterAroundPlayer ? "카메라가 주위를 회전합니다." : "플레이어의 시야에 따라서 회전합니다");
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
            //땅에 붙어있고 추락하고 있을 때 혹시 모를 상황을 위해 넣음
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

        moveSpeed = isRunning ? runSpeed : walkSpeed; //isRunning이 true면 runSpeed, false면 walkSpeed를 moveSpeed에 저장  
    }

    void AimSet()
    {
        if (Input.GetMouseButtonDown(1)) //우클릭시
        {
            

            isAim = true;
            //animator.SetBool("isAim", isAim);
            animator.SetLayerWeight(1, 1);

            if (zoomCoroutine != null) //중복방지, 코루틴이 작동 중인지 아닌지 확인 후 멈춤
            {
                StopCoroutine(zoomCoroutine);
            }

            if (isFirstPerson) //1인칭이라면(캐릭터 머리에 달림) 카메라 확대함
            {
                SetTargetFOV(zoomFov); //시야각 이동 확대함
                zoomCoroutine = StartCoroutine(ZoomFieldOfView(targetFov));
            }
            else //3인칭이라면 카메라 위치 이동, 배그 견착 느낌...?
            {
                SetTargetDistance(zoomDistance); //타겟 거리 세팅
                zoomCoroutine = StartCoroutine(ZoomCamera(targetDistance));
            }
        }

        if (Input.GetMouseButtonUp(1)) //뗐을 때 
        {
            isAim = false;
            //animator.SetBool("isAim", isAim);
            animator.SetLayerWeight(1, 0);

            if (zoomCoroutine != null) //중복방지
            {
                StopCoroutine(zoomCoroutine);
            }

            if (isFirstPerson) //1인칭
            {
                SetTargetFOV(defaultFov); //기본 시점으로 돌아감
                zoomCoroutine = StartCoroutine(ZoomFieldOfView(targetFov));
            }
            else //3인칭
            {
                SetTargetDistance(thirdPersonDistance); //뒤통수 뷰
                zoomCoroutine = StartCoroutine(ZoomCamera(targetDistance));
            }
        }
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Weapon Type MaxDistance Set 무기에 따라 최대 사정거리 세팅해야 함
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1번 누르면 주무기 장착
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

    void FirstPersonMovement() //1인칭 이동
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal; //카메라 시점이동 플레이어 시점이동과 동일시
        moveDirection.y = 0; //흔들림 방지, 없으면 버그가 생김
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime); //이동

        cameraTransform.position = playerHead.position; //카메라 시점은 플레이어 눈에서 보는거처럼 위치 세팅
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0);

        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0);
    }

    void ThirdPersonMovemnet() //3인칭 이동
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
            //카메라가 플레이어 오른쪽에서 회전하도록 설정
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

            //카메라를 플레이어의 오른쪽에서 고정된 위치로 이동
            cameraTransform.position = transform.position + thirdPersonOffset + rotation * direction;

            //카메라가 플레이어의 위치를 따라가도록 설정
            cameraTransform.LookAt(transform.position + new Vector3(0, thirdPersonOffset.y, 0)); //transform은 플레이어이기 때문에 나를 봐라!
        }
        else
        {
            //플레이어가 직접 회전하는 모드
            transform.rotation = Quaternion.Euler(0f, yaw, 0);
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            cameraTransform.position = playerLookObj.position + thirdPersonOffset + Quaternion.Euler(pitch, yaw, 0) * direction;
            cameraTransform.LookAt(playerLookObj.position + new Vector3(0, thirdPersonOffset.y, 0)); //카메라가 캐릭터를 뒤통수를 보면 안 되고 옆을 보고 있어야 해서

            UpdateAimTarget();
        }
    }

    public void SetTargetDistance(float distance) //타겟 거리를 설정하는 함수
    {
        targetDistance = distance;
    }

    public void SetTargetFOV(float fov) 
    {
        targetFov = fov;
    }

    /// <summary>
    /// 3인칭 줌에 사용
    /// </summary>
    /// <param name="targetDistance"></param>
    /// <returns></returns>
    IEnumerator ZoomCamera(float targetDistance)
    {
        while (Mathf.Abs(currentDistance - targetDistance) > 0.01f) //현재 거리에서 목표 거리로 부드럽게 이동
            {
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * ZoomSpeed);
            yield return null;
        }
        currentDistance = targetDistance; //목표 거리에 도달한 후 값을 고정
    }

    /// <summary>
    /// 1인칭 줌에 사용
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
            //animationSpeed = 2; //1번 방법
            FireSoundOn();
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("Damage");
            characterController.enabled = false;
            gameObject.transform.position = Vector3.zero;     
            characterController.enabled = true;
        }        
    }
}
