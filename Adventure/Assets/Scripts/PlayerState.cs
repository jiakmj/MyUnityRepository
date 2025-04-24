using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;

    [Header("플레이어 능력치")]
    public int maxHp = 4;
    public int currentHp;
    public int damage = 1;
    public float attackSpeed = 1.0f;
    public float moveSpeed = 3.0f;
    private bool isDead = false;
    private Vector3 initialPosition;

    private PlayerAnimation playerAnimation;
    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        initialPosition = transform.position;
        currentHp = maxHp;
    }

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        GameManager.Instance.LoadPlayerState(this);
        
    }

    void OnEnable()
    {
        isDead = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; 
       
        Debug.Log($"[데미지] {amount}만큼 입음 | 현재 체력: {currentHp}->{currentHp-amount}");

        SoundManager.Instance.PlaySFX(SFXType.HitSound);
        // 애니메이션 재생
        playerAnimation.TriggerHit();
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UIManager.Instance.UpdateHP(currentHp);

        if (currentHp <= 0)
        {
            // 죽음            
            animator.ResetTrigger("Hit");
            playerAnimation.TriggerDead();

            // 죽은 후 애니메이션이 끝날 때 씬 리로드
            StartCoroutine(HandleDeath());
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UIManager.Instance.UpdateHP(currentHp);
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    // 사망 애니메이션이 끝나면 씬을 리로드하는 코루틴
    public IEnumerator HandleDeath()
    {
        // 애니메이션 재생 시간만큼 기다리기 (대략적인 시간으로 설정)
        // 애니메이션의 길이를 정확히 알면 해당 시간으로 수정하면 좋음
        yield return new WaitForSeconds(2f); // 예: 2초 동안 대기 (애니메이션 시간에 맞춰 조정)

        // isDead 상태를 해제하고 초기 상태로 복구
        isDead = false;
        currentHp = maxHp;
        transform.position = initialPosition;
        playerAnimation.Reset(); // <- 필요 시 직접 구현
        animator.Rebind();
        animator.Update(0f);

        SceneController.Instance.StartSceneTransition(SceneManager.GetActiveScene().name);
        UIManager.Instance.UpdateHP(currentHp);
    }

    public void Die()
    {
        // GameOver창 시작
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public void UpgradeDamage(int amount)
    {
        damage += amount;
        GameManager.Instance.SavePlayerState(this);
    }
    
    public void UpgradeAttackSpeed(float amount)
    {
        attackSpeed += amount;
        GameManager.Instance.SavePlayerState(this);
    }

    public void UpgradeHP(int amount)
    {
        maxHp += amount;
        GameManager.Instance.SavePlayerState(this);
    }
    public void UpgradeMoveSpeed(float amount)
    {
        moveSpeed += amount;
        GameManager.Instance.SavePlayerState(this);
    }   
   
}
