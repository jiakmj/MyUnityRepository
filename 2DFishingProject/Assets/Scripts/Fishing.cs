using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{    
    public FishManager fishManager;
    //public GameObject player;
    public FishInfo fishInfo;

    private Fish currentFish;

    public Button fishingButton;
    public Slider progressBar; //게이지바
    private Image progressBarFill;

    public GameObject progressBarPosition;

    public int requestHit = 10; //연타 횟수
    private int hitCount = 0;
    private float timeLimit = 20.0f;
    private float startTime;
    public Transform player;

    Vector3 dir;

    void Start()
    {                
        fishingButton.onClick.AddListener(StartFishing);
        //progressBar.transform.SetParent(progressBarPosition.transform);
        progressBarFill = progressBar.fillRect.GetComponent<Image>();
        //progressBar.transform.localPosition = Vector3.zero;
        var bar = Instantiate(progressBar);
        bar.transform.position = player.position;
        progressBar.gameObject.SetActive(false);

        if (fishInfo == null)
        {
            fishInfo = FindObjectOfType<FishInfo>();
            if (fishInfo == null)
            {
                Debug.LogError("fishinfo 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }   

    public void StartFishing()
    {
        Debug.Log("클릭 성공 낚시 시작");

        if (currentFish != null)
        {
            Debug.Log($"현재 물고기: {currentFish.name}");
            StartCatching(currentFish);
        }
        else
        {
            Debug.Log("현재 물고기가 설정되지 않았습니다.");
            StartCoroutine(FishingProcess());
        }

        //fishAppearTime = Time.time + Random.Range(2f, 5f); //랜덤 시간
        //StartCoroutine(FishingProcess());
    }

    IEnumerator FishingProcess()
    {
        float fishAppearTime = Random.Range(2f, 10f); //랜덤 시간

        currentFish = fishManager.GetRandomFish(); //JSON에서 Fish 정보 불러옴       

        if (currentFish != null)
        {
            StartCatching(currentFish);
            Debug.Log($"물고기: {currentFish.name}");
        }
        else
        {
            Debug.Log("물고기를 가져오는데 실패했습니다.");
        }

        yield return new WaitForSeconds(fishAppearTime);
    }
    public void StartCatching(Fish fish)
    {
        Debug.Log("startCatching 호출");
        if (fish == null)
        {
            Debug.LogError("전달된 fish가 null입니다.");
            return;
        }        
        //isCatch = true;
        currentFish = fish;
        hitCount = 0;
        startTime = Time.time;
        progressBar.value = 0.0f;
        progressBar.gameObject.SetActive(true);
        //requestHit = fish.level * 5; //난이도 1~5정도로 셋팅
        fishInfo.ShowFishInfo(currentFish);
        StartCoroutine(CatchingFishProcess());
        
        if (fishInfo != null)
        {
            fishInfo.ShowFishInfo(currentFish);
        }
        else
        {
            Debug.Log("fishinfo가 null입니다.");
        }

    }
    IEnumerator CatchingFishProcess()
    {
        Debug.Log("catchingfishprocess 시작");
        while (Time.time - startTime < timeLimit)
        {
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                hitCount++;
                Debug.Log($"연타 횟수: {hitCount}/{requestHit}");

                progressBar.value = (float)hitCount / requestHit;
                UpdateProgressBarColor(progressBar.value);
            }

            if (hitCount >= requestHit)
            {
                Debug.Log("낚았다!");
                fishInfo.ShowFishInfo(currentFish);
                progressBar.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
        Debug.Log("미끼만 먹고 도망갔다...");
        fishInfo.HideFishInfo();
        progressBar.gameObject.SetActive(false);
    }
    private void UpdateProgressBarColor(float value)
    {
        if (value < 0.5f)
        {
            progressBarFill.color = Color.black;
        }
        else if (value < 0.8f)
        {
            progressBarFill.color = new Color(1.0f, 0.5f, 0.0f);
        }
        else
        {
            progressBarFill.color = Color.red;
        }
    }

}
