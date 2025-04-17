using JetBrains.Annotations;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;

    [Header("�÷��̾� �ɷ�ġ")]
    public int maxHp = 100;
    public int currentHp;
    public int damage = 10;
    public float attackSpeed = 1.0f;
    public float moveSpeed = 3.0f;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        currentHp = maxHp;
    }

    void Start()
    {
        GameManager.Instance.LoadPlayerState(this);
    }

    public void TakeDamage(int amount)
    {
        SoundManager.Instance.PlaySFX(SFXType.HitSound);
        // �ִϸ��̼� ���
        currentHp -= amount;
        if (currentHp <= 0)
        {
            // ����
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
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
