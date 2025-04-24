using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;

    [Header("�÷��̾� �ɷ�ġ")]
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
       
        Debug.Log($"[������] {amount}��ŭ ���� | ���� ü��: {currentHp}->{currentHp-amount}");

        SoundManager.Instance.PlaySFX(SFXType.HitSound);
        // �ִϸ��̼� ���
        playerAnimation.TriggerHit();
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UIManager.Instance.UpdateHP(currentHp);

        if (currentHp <= 0)
        {
            // ����            
            animator.ResetTrigger("Hit");
            playerAnimation.TriggerDead();

            // ���� �� �ִϸ��̼��� ���� �� �� ���ε�
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

    // ��� �ִϸ��̼��� ������ ���� ���ε��ϴ� �ڷ�ƾ
    public IEnumerator HandleDeath()
    {
        // �ִϸ��̼� ��� �ð���ŭ ��ٸ��� (�뷫���� �ð����� ����)
        // �ִϸ��̼��� ���̸� ��Ȯ�� �˸� �ش� �ð����� �����ϸ� ����
        yield return new WaitForSeconds(2f); // ��: 2�� ���� ��� (�ִϸ��̼� �ð��� ���� ����)

        // isDead ���¸� �����ϰ� �ʱ� ���·� ����
        isDead = false;
        currentHp = maxHp;
        transform.position = initialPosition;
        playerAnimation.Reset(); // <- �ʿ� �� ���� ����
        animator.Rebind();
        animator.Update(0f);

        SceneController.Instance.StartSceneTransition(SceneManager.GetActiveScene().name);
        UIManager.Instance.UpdateHP(currentHp);
    }

    public void Die()
    {
        // GameOverâ ����
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
