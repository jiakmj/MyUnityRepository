using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int coinCount = 0;

    private const string COIN_KEY = "CoinCount";
    private const string DAMAGE_KEY = "PlayerDamage";
    private const string ATTACK_SPEED_KEY = "PlayerAttackSpeed";
    private const string MOVE_SPEED_KEY = "PlayerMoveSpeed";
    private const string HP_KEY = "PlayerHP";

    [SerializeField] private GameObject[] blocksToRemove;
    private bool blockRemoved = false;

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
    }


    public void AddCoin(int amount)
    {
        coinCount += amount;
        if (coinCount > 3)
            coinCount = 3;
        //coinText.text = coinCount.ToString();
        SaveCoin();
        SoundManager.Instance.PlaySFX(SFXType.ItemGet);
        // PlayerPrefs.SetInt("Coin", coinCount);
        UIManager.Instance.UpdateCoinUI(coinCount);

        if (coinCount == 3 && !blockRemoved)
        {
            blockRemoved = true;
            RemoveBlockingObjects();
        }
    }

    public void ResetCoin()
    {
        coinCount = 0;
        //coinText.text = coinCount.ToString();
        UIManager.Instance.UpdateCoinUI(coinCount);
    }

    public bool UseCoin(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            SaveCoin();
            //UIManager.Instance.UpdateCoinUI(coinCount);
            return true;
        }
        Debug.Log("코인이 부족합니다.");
        return false;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt(COIN_KEY, coinCount);
        PlayerPrefs.Save();
    }

    private void LoadCoin()
    {
        coinCount = PlayerPrefs.GetInt(COIN_KEY, 0);
        //UIManager.Instance.UpdateCoinUI(coinCount);
    }

    public void SavePlayerState(PlayerState states)
    {
        PlayerPrefs.SetInt(DAMAGE_KEY, states.damage);
        PlayerPrefs.SetFloat(MOVE_SPEED_KEY, states.moveSpeed);
        PlayerPrefs.SetFloat(ATTACK_SPEED_KEY, states.attackSpeed);
        PlayerPrefs.SetFloat(HP_KEY, states.maxHp);
    }

    public void LoadPlayerState(PlayerState states)
    {
        if (PlayerPrefs.HasKey(DAMAGE_KEY))
        {
            states.damage = PlayerPrefs.GetInt(DAMAGE_KEY);
        }
        if (PlayerPrefs.HasKey(ATTACK_SPEED_KEY))
        {
            states.attackSpeed = PlayerPrefs.GetFloat(ATTACK_SPEED_KEY);
        }
        if (PlayerPrefs.HasKey(HP_KEY))
        {
            states.maxHp = PlayerPrefs.GetInt(HP_KEY);
        }
        if (PlayerPrefs.HasKey(MOVE_SPEED_KEY))
        {
            states.moveSpeed = PlayerPrefs.GetFloat(MOVE_SPEED_KEY);
        }
    }

    private void RemoveBlockingObjects()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach (var obj in blocks)
        {
            // Shake
            CameraShakeManager.Instance.Shake(0.3f, 0.2f);

            // Delay 후 파괴
            Destroy(obj, 0.3f);
        }
    }
}
