using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public Text damageText;
    public Text coinText;

    // �⺻ ���׷��̵� ���
    public int baseDamageCost = 25;
    public int baseAttackSpeedCost = 27;
    public int baseMoveSpeedCost = 10;
    public int baseHpCost = 10;

    // ���׷��̵� ��ġ
    public int damageUpgradeAmount = 5;
    public float attackSpeedUpgradeAmount = 0.2f;
    public float moveSpeedUpgradeAmount = 0.3f;
    public int hpUpgradeAmount = 10;

    // ���׷��̵� Ƚ�� ����
    private int damageUpgradeCount = 0;
    private int attackSpeedUpgradeCount = 0;
    private int moveSpeedUpgradeCount = 0;
    private int hpUpgradeCount = 0;

    // ���� ��� ����
    private const int increaseThreshold = 3; // 3 ȸ �̻��� �� ���� ����
    private const float priceIncreaseRate = 1.5f; // ��� * 1.5

    private int defaultMaxHp = 100;
    private int defaultDamage = 10;
    private float defaultAttackSpeed = 1.0f;
    private float defaultMoveSpeed = 3.0f;

    private PlayerState playerStates;


    void Start()
    {
        playerStates = PlayerState.Instance;
        UpdateUI();
    }

    private int GetCost(int baseCost, int upgradeCount)
    {
        if (upgradeCount < increaseThreshold)
        {
            return baseCost;
        }
        return Mathf.FloorToInt(baseCost * priceIncreaseRate); // �����������(�Ҽ��� ����)
    }
      
    public void UpgradeDamage()
    {
        UpdateUI();
        SoundManager.Instance.PlaySFX(SFXType.ItemGet);
        int cost = GetCost(baseDamageCost, damageUpgradeCount);
        if (GameManager.Instance.UseCoin(cost))
        {
            playerStates.UpgradeDamage(damageUpgradeAmount);
            damageUpgradeCount++;
        }
    }

    public void UpgradeAttackSpeed()
    {
        UpdateUI();
        int cost = GetCost(baseAttackSpeedCost, attackSpeedUpgradeCount);
        if (GameManager.Instance.UseCoin(cost))
        {
            playerStates.UpgradeAttackSpeed(attackSpeedUpgradeAmount);
            attackSpeedUpgradeCount++;
        }
    }

    public void UpgradeMoveSpeed()
    {
        UpdateUI();
        int cost = GetCost(baseMoveSpeedCost, moveSpeedUpgradeCount);
        if(GameManager.Instance.UseCoin(cost))
        {
            playerStates.UpgradeMoveSpeed(moveSpeedUpgradeAmount);
            moveSpeedUpgradeCount++;
        }
    }


    public void UpgradeHP()
    {
        UpdateUI();
        int cost = GetCost(baseHpCost, hpUpgradeCount);
        if (GameManager.Instance.UseCoin(cost))
        {
            playerStates.UpgradeHP(hpUpgradeAmount);
            hpUpgradeCount++;
        }
    }
    
    public void ResetState()
    {
        UpdateUI();
        if (GameManager.Instance.UseCoin(100))
        {
            playerStates.damage = defaultDamage;
            playerStates.maxHp = defaultMaxHp;
            playerStates.moveSpeed = defaultMoveSpeed;
            playerStates.attackSpeed = defaultAttackSpeed;

            GameManager.Instance.SavePlayerState(playerStates);
        }
    }

    public void UpdateUI()
    {
        damageText.text = playerStates.damage.ToString();
        coinText.text = GameManager.Instance.GetCoinCount().ToString();
    }
}
