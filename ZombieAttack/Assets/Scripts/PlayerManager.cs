using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations.Rigging; //NameSpace: �Ҽ�
using UnityEngine.UI;

//���⸦ ������ ����
public enum WeaponMode
{
    Pistol,
    Shotgun,
    Rifle,
}
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

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
    public AudioClip audioClipFootStep;
    public GameObject SMGObj;
    private int animationSpeed = 1; //�ִϸ��̼� �ӵ� ����
    private string currentAnimation = "Idle";

    public Transform aimTarget;

    private float weaponMaxDistance = 100.0f;

    public LayerMask TargetLayerMask;

    public MultiAimConstraint multiAimConstraint;

    public Vector3 boxSize = new Vector3(1.0f, 1.0f, 1.0f);
    public float castDistance = 5.0f;
    public LayerMask itemLayer;
    public Transform itemGetPos;

    //crosshair�� aim������ ���� ���̰� ����
    public GameObject crosshairObj;
    public GameObject SMGIconImage; //���߿� SMG�� �̹��� �ٲܰ�

    //�������� �Ծ�߸� ���� �� �� �ְ�, ���Ⱑ 1���� ���
    private bool isUseWeapon = false;
    private bool isGetSMGItem = false;

    //��ƼŬ ����Ʈ ���
    public ParticleSystem SMGEffect;

    private float gunFireDelay = 0.5f;

    public ParticleSystem DamageParticleSystem;
    public AudioClip audioClipDamage;

    public Text bulletText;
    private int firebulletCount = 0; //������ �Ѿ�
    private int savebulletCount = 0; //������ �ִ� �Ѿ�

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
    private float recoilStrength = 2.0f; //�ݵ�
    private float maxRecoilAngle = 10.0f; //�ݵ��� �ִ� ����
    private float currentRecoil = 0.0f; 
    private float shakeDuration = 0.1f; //��鸲
    private float shakeMagnitude = 0.1f; //��鸲 ����
    private Vector3 originalCameraPosition; //ī�޶� �������� �⺻ ī�޶� ��ġ
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
        mainCamera.fieldOfView = defaultFov; //����, �ܾƿ��� �ϱ� ���ؼ�
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


        //1�� ���
        animator.speed = animationSpeed;

        //2�� ���
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(stateInfo.IsName("HitReaction"));
        //if (stateInfo.IsName(currentAnimation))
        //{
        //    Debug.Log("stateInfo.normalizedTime : " + stateInfo.normalizedTime);
        //}

        if (stateInfo.IsName(currentAnimation) && stateInfo.normalizedTime >= 1.0f) //�ִϸ��̼��� �� ���̸� 1�� ���� 1.0�� ������ �� �ִϸ��̼��� ������ �ǹ��� 
        {
            //�� �ִϸ��̼��� ������ �ٸ� �ִϸ��̼��� ���� ������
            currentAnimation = "Attack";
            animator.Play(currentAnimation);
        }

        if (currentRecoil > 0 )
        {
            //���� ���� �� �ݵ����� ���� ���� �ö󰡴� �ڵ�
            currentRecoil -= recoilStrength * Time.deltaTime; //�ݵ��� ������ ����
            currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle);
            Quaternion currentRotation = Camera.main.transform.rotation;
            Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0);
            Camera.main.transform.rotation = currentRotation * recoilRotation; //ī�޶� �����ϴ� �ڵ带 �����Ѵ�.

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
        Quaternion currentRotation = Camera.main.transform.rotation; //���� ī�޶� ���� ȸ���� ��������
        Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0); //�ݵ��� ����ؼ� x�� ���� ȸ���� �߰�
        Camera.main.transform.rotation = currentRotation * recoilRotation; //���� ȸ�� ���� �ݵ��� ���Ͽ� ���ο� ȸ����
        currentRecoil += recoilStrength;
        currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle); //�ݵ����� ����
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
        Camera.main.transform.position = originalPosition; //���� ��ġ�� ���ư�
    }


    void UpdateAimTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ī�޶��� ���鿡�� ���콺 �����͸� �������� ������ �߻��ϴ°�
        aimTarget.position = ray.GetPoint(10.0f); //�Ÿ� ������

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
        //�ݴ� ��� ����
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
                Debug.Log("����");
                if (hit.collider.GetComponent<DoorManager>().isOpen)
                {
                    Debug.Log("�����ο���");
                    hit.collider.GetComponent<Animator>().SetTrigger("OpenForward");
                }
                else
                {
                    Debug.Log("�ڷο���");
                    hit.collider.GetComponent<Animator>().SetTrigger("OpenBackward");                
                }
            }
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
        if (Input.GetMouseButtonDown(1) && isGetSMGItem && isUseWeapon) //��Ŭ����
        {
            isAim = true;
            multiAimConstraint.data.offset = new Vector3(-40, 0, 0);
            crosshairObj.SetActive(true);
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

        if (Input.GetMouseButtonUp(1) && isGetSMGItem && isUseWeapon) //���� �� 
        {
            isAim = false;
            multiAimConstraint.data.offset = new Vector3(0, 0, 0);
            crosshairObj.SetActive(false);
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

    void Reloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading) //RŰ ������ ����
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
                //���⸦ �ٲ۴ٸ�
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

                        //Weapon Type MaxDistance Set ���⿡ ���� �ִ� �����Ÿ� �����ؾ� ��
                        weaponMaxDistance = 1000.0f;

                        isFire = true;
                        //gunFireDelay = 1.0f;

                        //Weapon Type FireDelay felax fix���� Ÿ�� ������ ����
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

                    //Weapon Type MaxDistance Set ���⿡ ���� �ִ� �����Ÿ� �����ؾ� ��
                    weaponMaxDistance = 1000.0f;

                    isFire = true;
                    //gunFireDelay = 1.0f;

                    //Weapon Type FireDelay felax fix���� Ÿ�� ������ ����
                    StartCoroutine(FireWithDelay(gunFireDelay));
                    animator.SetTrigger("Fire");

                    //�ݵ��߰�
                    ApplyRecoil();
                    StartCameraShake();

                    Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
                    RaycastHit[] hits = Physics.RaycastAll(ray, weaponMaxDistance, TargetLayerMask);

                    if (hits.Length > 0)
                    {
                        for (int i = 0; i < hits.Length && i < 2; i++)
                        {
                            Debug.Log("�浹 : " + hits[i].collider.name);

                            //ParticleManager.Instance.ParticlePlay(ParticleType.DamageExplosion, hits[i].point, Vector3.one);
                            ParticleSystem particle = Instantiate(DamageParticleSystem, hits[i].point, Quaternion.identity); //������ �����ؼ� ���
                            DamageParticleSystem.transform.position = hits[i].point; //���� ��ġ���� ��ƼŬ ������
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
                    //�Ѿ��� ���� �Ҹ� ���
                    SoundManager.Instance.PlaySFX("GunEmptySound", transform.position);
                    //audioSource.PlayOneShot(audioClipBlankAmmo);
                    Debug.Log("�Ѿ˾���");                    
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
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGetSMGItem) //1�� ������ �ֹ��� ����
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
        Time.timeScale = 1; //���� �ð� �簳
    }

    public void Pause()
    {
        PauseObj.SetActive(true);
        Time.timeScale = 0; //���� �ð� ����
        
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
        //audioSource.PlayOneShot(audioClipFootStep); //�߼Ҹ����       
    }

    public void ReloadingSoundOn()
    {
        SoundManager.Instance.PlaySFX("GunReloadSound", transform.position);
        //audioSource.PlayOneShot(audioClipReload); //�����Ҹ� ���    
    }

    public void HitSoundOn()
    {
        SoundManager.Instance.PlaySFX("PlayerPainSound", transform.position);
        //audioSource.PlayOneShot(audioClipHit); //�´¼Ҹ� ���    
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
        //} //�������� ������ �÷��̾��� �ڽ����� �߰��ϰ� �� ���� �ڽĿ��� ����        
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
