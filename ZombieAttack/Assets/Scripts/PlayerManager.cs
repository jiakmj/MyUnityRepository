using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations.Rigging; //NameSpace: 소속
using UnityEngine.UI;

//무기를 여러개 쓰면
public enum WeaponMode
{
    Pistol,
    Shotgun,
    Rifle,
}
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

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
    public AudioClip audioClipFootStep;
    public GameObject SMGObj;
    private int animationSpeed = 1; //애니메이션 속도 조절
    private string currentAnimation = "Idle";

    public Transform aimTarget;

    private float weaponMaxDistance = 100.0f;

    public LayerMask TargetLayerMask;

    public MultiAimConstraint multiAimConstraint;

    public Vector3 boxSize = new Vector3(1.0f, 1.0f, 1.0f);
    public float castDistance = 5.0f;
    public LayerMask itemLayer;
    public Transform itemGetPos;

    //crosshair가 aim상태일 때만 보이게 설정
    public GameObject crosshairObj;
    public GameObject SMGIconImage; //나중에 SMG로 이미지 바꿀것

    //아이템을 먹어야만 총을 들 수 있게, 무기가 1개인 경우
    private bool isUseWeapon = false;
    private bool isGetSMGItem = false;

    //파티클 이펙트 등록
    public ParticleSystem SMGEffect;

    private float gunFireDelay = 0.5f;

    public ParticleSystem DamageParticleSystem;
    public AudioClip audioClipDamage;

    public Text bulletText;
    private int firebulletCount = 0; //장전한 총알
    private int savebulletCount = 0; //가지고 있는 총알

    public GameObject flashLightObj;
    private bool isFlashLightOn = false;
    public AudioClip audioClipFlashOn;

    private float playerHp = 100;
    private bool isDead = false;

    private bool isReloading = false;
    public AudioClip audioClipReload;
    public AudioClip audioClipBlankAmmo;
    public AudioClip audioClipHit;

    public GameObject PauseObj;
    private bool isPause = false;

    private WeaponMode currentWeaponMode = WeaponMode.Rifle;
    private int ShotgunRayCount = 5;
    private float shotGunSpreadAngle = 10.0f;
    private float recoilStrength = 2.0f; //반동
    private float maxRecoilAngle = 10.0f; //반동의 최대 각도
    private float currentRecoil = 0.0f; 
    private float shakeDuration = 0.1f; //흔들림
    private float shakeMagnitude = 0.1f; //흔들림 강도
    private Vector3 originalCameraPosition; //카메라가 흔들렸으니 기본 카메라 위치
    private Coroutine cameraShakeCoroutine;   


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

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
        crosshairObj.SetActive(false);
        SMGIconImage.SetActive(false);
        bulletText.text = $"{firebulletCount}/{savebulletCount}";
        bulletText.gameObject.SetActive(false);
        flashLightObj.SetActive(false);
        PauseObj.SetActive(false);

        
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

        Operate();

        Reloading();

        ActionFlashLight();

        GameMenu();


        //1번 방법
        animator.speed = animationSpeed;

        //2번 방법
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(stateInfo.IsName("HitReaction"));
        //if (stateInfo.IsName(currentAnimation))
        //{
        //    Debug.Log("stateInfo.normalizedTime : " + stateInfo.normalizedTime);
        //}

        if (stateInfo.IsName(currentAnimation) && stateInfo.normalizedTime >= 1.0f) //애니메이션의 총 길이를 1로 보고 1.0이 지나면 이 애니메이션이 끝남을 의미함 
        {
            //이 애니메이션이 끝나면 다른 애니메이션을 실행 시켜줘
            currentAnimation = "Attack";
            animator.Play(currentAnimation);
        }

        if (currentRecoil > 0 )
        {
            //총을 쐈을 때 반동으로 인해 위로 올라가는 코드
            currentRecoil -= recoilStrength * Time.deltaTime; //반동을 서서히 줄임
            currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle);
            Quaternion currentRotation = Camera.main.transform.rotation;
            Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0);
            Camera.main.transform.rotation = currentRotation * recoilRotation; //카메라를 제어하는 코드를 꺼야한다.

        }
    }

    void FireShotgun()
    {
        for (int i = 0; i < ShotgunRayCount; i++)
        {
            RaycastHit hit;

            Vector3 origin = Camera.main.transform.position;
            Vector3 spreadDirection = GetSpreadDirection(Camera.main.transform.forward, shotGunSpreadAngle);
            Debug.DrawRay(origin, spreadDirection * castDistance, Color.blue, 2.0f);
            if (Physics.Raycast(origin, spreadDirection, out hit, castDistance, TargetLayerMask))
            {
                Debug.Log("Shotgun Hit : " + hit.collider.name);
            }
        }
    }
    Vector3 GetSpreadDirection(Vector3 forwardDirection, float spreadAngle)
    {
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);
        Vector3 spreadDirection = Quaternion.Euler(spreadX, spreadY, 0) * forwardDirection;
        return spreadDirection;
    }

    void ApplyRecoil()
    {
        Quaternion currentRotation = Camera.main.transform.rotation; //현재 카메라 월드 회전값 가져오기
        Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0); //반동을 계산해서 x축 상하 회전에 추가
        Camera.main.transform.rotation = currentRotation * recoilRotation; //현재 회전 값에 반동을 곱하여 새로운 회전값
        currentRecoil += recoilStrength;
        currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle); //반동값을 제한
    }


    void StartCameraShake()
    {
        if (cameraShakeCoroutine != null)
        {
            StopCoroutine(cameraShakeCoroutine);
        }
        cameraShakeCoroutine = StartCoroutine(CameraShake(shakeDuration, shakeMagnitude));
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        Vector3 originalPosition = Camera.main.transform.position;
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1.0f, 1.0f) * magnitude;
            float offsetY = Random.Range(-1.0f, 1.0f) * magnitude;

            Camera.main.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0.0f);

            elapsed += Time.deltaTime;

            yield return null;
        }
        Camera.main.transform.position = originalPosition; //원래 위치로 돌아감
    }


    void UpdateAimTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //카메라의 정면에서 마우스 포인터를 중점으로 레이저 발사하는거
        aimTarget.position = ray.GetPoint(10.0f); //거리 정해줌

    }

    void Operate()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.E) && !stateInfo.IsName("PickUp"))
        {
            animator.SetTrigger("Operate");
        }
    }

    public void ItemBoxCast()
    {
        //줍는 대상 설정
        Vector3 origin = itemGetPos.position;
        Vector3 direction = itemGetPos.forward;
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(origin, boxSize / 2, direction, Quaternion.identity, castDistance, itemLayer);
        DebugBox(origin, direction);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.name == "ItemSMG")
            {
                hit.collider.gameObject.SetActive(false);
                audioSource.PlayOneShot(audioClipPickUp);
                SMGIconImage.SetActive(true);
                Debug.Log("Item : " + hit.collider.name);
                isGetSMGItem = true;
                bulletText.gameObject.SetActive(true);
            }
            else if (hit.collider.name == "ItemBullet")
            {                               
                hit.collider.gameObject.SetActive(false);
                audioSource.PlayOneShot(audioClipPickUp);
                savebulletCount += 30;
                if (savebulletCount >= 120)
                {
                    savebulletCount = 120;
                }
                //isBulletItem = true;
                bulletText.text = $"{firebulletCount}/{savebulletCount}";
                bulletText.gameObject.SetActive(true);
            }
            else if (hit.collider.name == "Door")
            {
                Debug.Log("도어");
                if (hit.collider.GetComponent<DoorManager>().isOpen)
                {
                    Debug.Log("앞으로열기");
                    hit.collider.GetComponent<Animator>().SetTrigger("OpenForward");
                }
                else
                {
                    Debug.Log("뒤로열기");
                    hit.collider.GetComponent<Animator>().SetTrigger("OpenBackward");                
                }
            }
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
        if (Input.GetMouseButtonDown(1) && isGetSMGItem && isUseWeapon) //우클릭시
        {
            isAim = true;
            multiAimConstraint.data.offset = new Vector3(-40, 0, 0);
            crosshairObj.SetActive(true);
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

        if (Input.GetMouseButtonUp(1) && isGetSMGItem && isUseWeapon) //뗐을 때 
        {
            isAim = false;
            multiAimConstraint.data.offset = new Vector3(0, 0, 0);
            crosshairObj.SetActive(false);
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

    void Reloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading) //R키 누르면 장전
        {
            if (savebulletCount > 0 && firebulletCount < 30)
            {
                isReloading = true;                               

                int neededBullets = 30 - firebulletCount;

                if (savebulletCount >= neededBullets)
                {
                    firebulletCount += neededBullets;
                    savebulletCount -= neededBullets;
                }
                else
                {
                    firebulletCount += savebulletCount;
                    savebulletCount = 0;
                }

                bulletText.text = $"{firebulletCount}/{savebulletCount}";
                bulletText.gameObject.SetActive(true);
                Debug.Log($"{firebulletCount}/{savebulletCount}");

                isReloading = false;
            }
            animator.SetTrigger("isWeaponReload");
        }
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isReloading)
            {
                return;
            }

            if (isAim && !isFire)
            {
                //무기를 바꾼다면
                if (currentWeaponMode == WeaponMode.Pistol)
                {

                }
                else if (currentWeaponMode == WeaponMode.Shotgun)
                {
                    if (firebulletCount > 0)
                    {
                        firebulletCount -= 1;
                        bulletText.text = $"{firebulletCount}/{savebulletCount}";
                        bulletText.gameObject.SetActive(true);

                        //Weapon Type MaxDistance Set 무기에 따라 최대 사정거리 세팅해야 함
                        weaponMaxDistance = 1000.0f;

                        isFire = true;
                        //gunFireDelay = 1.0f;

                        //Weapon Type FireDelay felax fix무기 타입 딜레이 수정
                        StartCoroutine(FireWithDelay(gunFireDelay));
                        animator.SetTrigger("Fire");

                        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
                        RaycastHit[] hits = Physics.RaycastAll(ray, weaponMaxDistance, TargetLayerMask);
                    }
                    FireShotgun();
                }
                else if (currentWeaponMode == WeaponMode.Rifle)
                {

                }

                if (firebulletCount > 0)
                {
                    firebulletCount -= 1;
                    bulletText.text = $"{firebulletCount}/{savebulletCount}";
                    bulletText.gameObject.SetActive(true);

                    //Weapon Type MaxDistance Set 무기에 따라 최대 사정거리 세팅해야 함
                    weaponMaxDistance = 1000.0f;

                    isFire = true;
                    //gunFireDelay = 1.0f;

                    //Weapon Type FireDelay felax fix무기 타입 딜레이 수정
                    StartCoroutine(FireWithDelay(gunFireDelay));
                    animator.SetTrigger("Fire");

                    //반동추가
                    ApplyRecoil();
                    StartCameraShake();

                    Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
                    RaycastHit[] hits = Physics.RaycastAll(ray, weaponMaxDistance, TargetLayerMask);

                    if (hits.Length > 0)
                    {
                        for (int i = 0; i < hits.Length && i < 2; i++)
                        {
                            Debug.Log("충돌 : " + hits[i].collider.name);

                            //ParticleManager.Instance.ParticlePlay(ParticleType.DamageExplosion, hits[i].point, Vector3.one);
                            ParticleSystem particle = Instantiate(DamageParticleSystem, hits[i].point, Quaternion.identity); //프리팹 복제해서 재생
                            DamageParticleSystem.transform.position = hits[i].point; //맞은 위치에서 파티클 나오게
                            particle.Play();
                            SoundManager.Instance.PlaySFX("zombieGrowl", transform.position);
                            //audioSource.PlayOneShot(audioClipDamage);

                            Debug.DrawLine(ray.origin, hits[i].point, Color.red, 3.0f);
                            hits[i].collider.GetComponent<ZombieManager>().TakeDamage(30.0f);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(ray.origin, ray.origin + ray.direction * weaponMaxDistance, Color.green, 3.0f);
                    }
                }
                else
                {
                    //총알이 없는 소리 재생
                    SoundManager.Instance.PlaySFX("GunEmptySound", transform.position);
                    //audioSource.PlayOneShot(audioClipBlankAmmo);
                    Debug.Log("총알없음");                    
                }
            }
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    isFire = false;            
        //}
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        playerHp -= damage;

        if (playerHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");

        //GameManager.Instance.GameOver();
        gameObject.SetActive(false);
    }

    void ChangeTools()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGetSMGItem) //1번 누르면 주무기 장착
        {
            animator.SetTrigger("isWeaponChange");
            SMGObj.SetActive(true);
            isUseWeapon = true;
        }
    }

    void ActionFlashLight()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            audioSource.PlayOneShot(audioClipFlashOn);
            isFlashLightOn = !isFlashLightOn;
            flashLightObj.SetActive(isFlashLightOn);
        }
    }

    private void GameMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;

            if (isPause)
            {
                Pause();
            }
            else
            {
                ReGame();
            }
        }
    }

    public void ReGame()
    {
        PauseObj.SetActive(false);
        Time.timeScale = 1; //게임 시간 재개
    }

    public void Pause()
    {
        PauseObj.SetActive(true);
        Time.timeScale = 0; //게임 시간 정지
        
    }

    public void Exit()
    {
        PauseObj.SetActive(false);
        Time.timeScale = 0;
        Application.Quit();
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
    
    IEnumerator FireWithDelay(float fireDelay)
    {
        yield return new WaitForSeconds(fireDelay);
        isFire = false;  
    }

    public void WeaponChangeSoundOn()
    {
        SoundManager.Instance.PlaySFX("GunEmptySound", transform.position);
        //audioSource.PlayOneShot(audioClipWeaponChange);
    }

    public void FireSoundOn()
    {
        SoundManager.Instance.PlaySFX("GunFireSound", transform.position);
        //audioSource.PlayOneShot(audioClipFire);
        SMGEffect.Play();
    }

    public void PickUpSoundOn()
    {
        SoundManager.Instance.PlaySFX("PickUpSound", transform.position);
        //audioSource.PlayOneShot(audioClipPickUp);
    }

    public void FootStepSoundOn()
    {
        SoundManager.Instance.PlaySFX("LeftFootStepSound", transform.position);
        //audioSource.PlayOneShot(audioClipFootStep); //발소리재생       
    }

    public void ReloadingSoundOn()
    {
        SoundManager.Instance.PlaySFX("GunReloadSound", transform.position);
        //audioSource.PlayOneShot(audioClipReload); //장전소리 재생    
    }

    public void HitSoundOn()
    {
        SoundManager.Instance.PlaySFX("PlayerPainSound", transform.position);
        //audioSource.PlayOneShot(audioClipHit); //맞는소리 재생    
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
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("Damage");
            characterController.enabled = false;
            gameObject.transform.position = Vector3.zero;     
            characterController.enabled = true;
            playerHp -= 30;
        }               
        else if (other.gameObject.name == "Attack")
        {            
            animator.SetTrigger("Damage");            
            playerHp -= 30;
        }
        //else if (other.CompareTag("Item"))
        //{
        //    other.gameObject.transform.SetParent(transform);
        //    other.gameObject.transform.SetParent(itemGetPos);
        //    other.gameObject.transform.SetParent(null);
        //} //아이템을 먹으면 플레이어의 자식으로 추가하고 다 쓰면 자식에서 나옴        
    }

    void DebugBox(Vector3 origin, Vector3 direction)
    {
        Vector3 endPoint = origin + direction * castDistance;

        Vector3[] corners = new Vector3[8];
        corners[0] = origin + new Vector3(-boxSize.x, -boxSize.y, -boxSize.z) / 2;
        corners[1] = origin + new Vector3(boxSize.x, -boxSize.y, -boxSize.z) / 2;
        corners[2] = origin + new Vector3(-boxSize.x, boxSize.y, -boxSize.z) / 2;
        corners[3] = origin + new Vector3(boxSize.x, boxSize.y, -boxSize.z) / 2;
        corners[4] = origin + new Vector3(-boxSize.x, -boxSize.y, boxSize.z) / 2;
        corners[5] = origin + new Vector3(boxSize.x, -boxSize.y, boxSize.z) / 2;
        corners[6] = origin + new Vector3(-boxSize.x, boxSize.y, boxSize.z) / 2;
        corners[7] = origin + new Vector3(boxSize.x, boxSize.y, boxSize.z) / 2;

        Debug.DrawLine(corners[0], corners[1], Color.green, 3.0f);
        Debug.DrawLine(corners[1], corners[3], Color.green, 3.0f);
        Debug.DrawLine(corners[3], corners[2], Color.green, 3.0f);
        Debug.DrawLine(corners[2], corners[0], Color.green, 3.0f);
        Debug.DrawLine(corners[4], corners[5], Color.green, 3.0f);
        Debug.DrawLine(corners[5], corners[7], Color.green, 3.0f);
        Debug.DrawLine(corners[7], corners[6], Color.green, 3.0f);
        Debug.DrawLine(corners[6], corners[4], Color.green, 3.0f);
        Debug.DrawLine(corners[0], corners[4], Color.green, 3.0f);
        Debug.DrawLine(corners[1], corners[5], Color.green, 3.0f);
        Debug.DrawLine(corners[2], corners[6], Color.green, 3.0f);
        Debug.DrawLine(corners[3], corners[7], Color.green, 3.0f);
        Debug.DrawRay(origin, direction * castDistance, Color.green);
    }
}
