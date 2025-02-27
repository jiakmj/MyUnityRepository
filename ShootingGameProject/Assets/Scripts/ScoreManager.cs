using UnityEngine;
using UnityEngine.UI;

//점수를 관리하는 유일한 매니저
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

        currentScoreUI.text = $"현재 점수 : {currentScore}";
        bestScoreUI.text = $"최고 점수 : {bestScore}";
        
    }

    //매니저 내에 존재하는 필드 값(Inspector)
    public Text currentScoreUI;
    public Text bestScoreUI;
    private int currentScore;
    private int bestScore;

    //현재 점수에 대한 프로퍼티 설계 (Property)
    //값에 대한 접근과 설정을 변수처럼 진행할 수 있습니다.
    public int Score
    {
        get
        {
            return currentScore;
        }

        set
        {
            //1. 전달받은 값이 현재의 점수로 설정됩니다.
            currentScore = value;
            //2. UI에 해당 값이 적용됩니다.
            currentScoreUI.text = $"현재 점수 : {currentScore}";

            //현재 점수가 최고 점수를 넘었다면
            if (currentScore > bestScore)
            {
                //그 점수가 최고 점수로 설정되며
                bestScore = currentScore;
                //UI에 갱신됩니다.
                bestScoreUI.text = $"최고 점수 : {bestScore}";
                PlayerPrefs.SetInt("Bast Score", bestScore);
            }
        }
    }
}

