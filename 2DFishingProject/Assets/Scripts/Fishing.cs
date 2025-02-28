using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public FishManager fishManager;
    public FishInfo fishInfo;
    private Fish currentFish;

    public Button fishingButton;
    public Slider progressBar; //게이지바
    private Image progressBarFill;

    public GameObject progressBarPosition;

    public int requestHit; //연타 횟수
    private int hitCount = 0;
    private float timeLimit = 8.0f;
    private float startTime;

    public Transform player;

    private Coroutine fishingCoroutine;
    private Coroutine catchingFishCoroutine;

    public string state;


    void Start()
    {
        state = "playing";
        fishingButton.onClick.AddListener(StartFishing);
        progressBarFill = progressBar.fillRect.GetComponent<Image>();
        progressBar.gameObject.SetActive(false);


        if (fishInfo == null)
        {
            fishInfo = Object.FindFirstObjectByType<FishInfo>();
            if (fishInfo == null)
            {
                Debug.LogError("fishinfo 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }

    void Update()
    {
        if (state == "fishing" || state == "catching")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EscapeFishing();
            }
        }


        if (state == "catching" && progressBar.gameObject.activeSelf)
        {
            Vector3 positionPr = player.position + new Vector3(0, 2f, 0);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(positionPr);
            progressBar.transform.position = screenPosition;
        }
    }


    public void StartFishing()
    {
        if (state != "playing")
        {
            Debug.Log("현재 상태가 playing이 아니므로 낚시를 실행할 수 없습니다.");
            return;
        }

        state = "fishing";

        Debug.Log("클릭 성공 낚시 시작");

        player.GetComponent<Player>().StartFishing();

        fishingCoroutine = StartCoroutine(FishingProcess());

        //currentFish = fishManager.GetRandomFish();

        //if (currentFish != null)
        //{
        //    Debug.Log($"현재 물고기: {currentFish.name}");
        //    StartCatching(currentFish);
        //}
        //else
        //{
        //    Debug.Log("현재 물고기가 설정되지 않았습니다.");
        //    fishingCoroutine = StartCoroutine(FishingProcess());
        //}

        //fishAppearTime = Time.time + Random.Range(2f, 5f); //랜덤 시간
        //StartCoroutine(FishingProcess());
    }
    private void EscapeFishing()
    {
        if (state != "fishing" && state != "catching")
        {
            return;
        }

        Debug.Log("낚시 취소1");

        if (fishingCoroutine != null)
        {
            StopCoroutine(FishingProcess());
        }

        if (catchingFishCoroutine != null)
        {
            StopCoroutine(CatchingFishProcess());
        }

        progressBar.gameObject.SetActive(false);
        state = "playing";

        player.GetComponent<Player>().EndFishing();
        player.GetComponent<Player>().EnableMovement();
    }

    //public void EndFishing()
    //{
    //    state = "playing";
    //    progressBar.gameObject.SetActive(false);

    //    player.GetComponent<Player>().EndFishing();
    //    player.GetComponent<Player>().EnableMovement();
    //}

    IEnumerator FishingProcess()
    {
        Debug.Log("FishingProcess 실행");

        float fishAppearTime = Random.Range(5f, 10f); //랜덤 시간

        Debug.Log($"물고기 등장 대기 시간: {fishAppearTime}초");

        float elapsedTime = 0.0f;

        while (elapsedTime < fishAppearTime)
        {
            if (state != "fishing")  // ESC 눌러서 낚시 중지 시 바로 탈출
            {
                Debug.Log("낚시 취소 가능 물고기 등장 전");
                progressBar.gameObject.SetActive(false);
                state = "playing";
                yield break;  // 즉시 종료
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (state == "fishing")
        {
            currentFish = fishManager.GetRandomFish();
            Debug.Log($"물고기 등장: {currentFish.name}");
            StartCatching(currentFish);
        }        
        //else
        //{
        //    Debug.Log("낚시 중 상대가 fishing이 아닌 경우 낚시 취소");           
        //}
    }
   
    public void StartCatching(Fish fish)
    {
        Debug.Log("startCatching 호출");

        if (fish == null)
        {
            Debug.LogError("전달된 fish가 null입니다.");
            return;
        }

        if (state != "fishing")
        {
            return;
        }

        currentFish = fish;
        hitCount = 0;
        startTime = Time.time;
        progressBar.value = 0.0f;
        progressBar.gameObject.SetActive(true);

        requestHit = Mathf.Max(5, fish.level * 3);

        Debug.Log($"연타 횟수 설정: {requestHit}");

        state = "catching";

        catchingFishCoroutine = StartCoroutine(CatchingFishProcess());
    }

    IEnumerator CatchingFishProcess()
    {
        Debug.Log("catchingfishprocess 시작, 물었다!");

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

                state = "playing";

                player.GetComponent<Player>().EnableMovement();

                yield break;
            }
            yield return null;
        }

        Debug.Log("미끼만 먹고 도망갔다...");
        fishInfo.HideFishInfo();
        progressBar.gameObject.SetActive(false);
        state = "playing";
        player.GetComponent<Player>().EnableMovement();
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
