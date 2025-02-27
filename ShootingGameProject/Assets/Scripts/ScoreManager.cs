using UnityEngine;
using UnityEngine.UI;

//������ �����ϴ� ������ �Ŵ���
public class ScoreManager : MonoBehaviour
{
    #region Singleton
    public static ScoreManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        PlayerPrefs.GetInt("Best Score");

        currentScoreUI.text = $"���� ���� : {currentScore}";
        bestScoreUI.text = $"�ְ� ���� : {bestScore}";
        
    }

    //�Ŵ��� ���� �����ϴ� �ʵ� ��(Inspector)
    public Text currentScoreUI;
    public Text bestScoreUI;
    private int currentScore;
    private int bestScore;

    //���� ������ ���� ������Ƽ ���� (Property)
    //���� ���� ���ٰ� ������ ����ó�� ������ �� �ֽ��ϴ�.
    public int Score
    {
        get
        {
            return currentScore;
        }

        set
        {
            //1. ���޹��� ���� ������ ������ �����˴ϴ�.
            currentScore = value;
            //2. UI�� �ش� ���� ����˴ϴ�.
            currentScoreUI.text = $"���� ���� : {currentScore}";

            //���� ������ �ְ� ������ �Ѿ��ٸ�
            if (currentScore > bestScore)
            {
                //�� ������ �ְ� ������ �����Ǹ�
                bestScore = currentScore;
                //UI�� ���ŵ˴ϴ�.
                bestScoreUI.text = $"�ְ� ���� : {bestScore}";
                PlayerPrefs.SetInt("Bast Score", bestScore);
            }
        }
    }
}

