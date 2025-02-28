using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public FishManager fishManager;
    public FishInfo fishInfo;
    private Fish currentFish;

    public Button fishingButton;
    public Slider progressBar; //��������
    private Image progressBarFill;

    public GameObject progressBarPosition;

    public int requestHit; //��Ÿ Ƚ��
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
                Debug.LogError("fishinfo ������Ʈ�� ã�� �� �����ϴ�.");
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
            Debug.Log("���� ���°� playing�� �ƴϹǷ� ���ø� ������ �� �����ϴ�.");
            return;
        }

        state = "fishing";

        Debug.Log("Ŭ�� ���� ���� ����");

        player.GetComponent<Player>().StartFishing();

        fishingCoroutine = StartCoroutine(FishingProcess());

        //currentFish = fishManager.GetRandomFish();

        //if (currentFish != null)
        //{
        //    Debug.Log($"���� �����: {currentFish.name}");
        //    StartCatching(currentFish);
        //}
        //else
        //{
        //    Debug.Log("���� ����Ⱑ �������� �ʾҽ��ϴ�.");
        //    fishingCoroutine = StartCoroutine(FishingProcess());
        //}

        //fishAppearTime = Time.time + Random.Range(2f, 5f); //���� �ð�
        //StartCoroutine(FishingProcess());
    }
    private void EscapeFishing()
    {
        if (state != "fishing" && state != "catching")
        {
            return;
        }

        Debug.Log("���� ���1");

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
        Debug.Log("FishingProcess ����");

        float fishAppearTime = Random.Range(5f, 10f); //���� �ð�

        Debug.Log($"����� ���� ��� �ð�: {fishAppearTime}��");

        float elapsedTime = 0.0f;

        while (elapsedTime < fishAppearTime)
        {
            if (state != "fishing")  // ESC ������ ���� ���� �� �ٷ� Ż��
            {
                Debug.Log("���� ��� ���� ����� ���� ��");
                progressBar.gameObject.SetActive(false);
                state = "playing";
                yield break;  // ��� ����
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (state == "fishing")
        {
            currentFish = fishManager.GetRandomFish();
            Debug.Log($"����� ����: {currentFish.name}");
            StartCatching(currentFish);
        }        
        //else
        //{
        //    Debug.Log("���� �� ��밡 fishing�� �ƴ� ��� ���� ���");           
        //}
    }
   
    public void StartCatching(Fish fish)
    {
        Debug.Log("startCatching ȣ��");

        if (fish == null)
        {
            Debug.LogError("���޵� fish�� null�Դϴ�.");
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

        Debug.Log($"��Ÿ Ƚ�� ����: {requestHit}");

        state = "catching";

        catchingFishCoroutine = StartCoroutine(CatchingFishProcess());
    }

    IEnumerator CatchingFishProcess()
    {
        Debug.Log("catchingfishprocess ����, ������!");

        while (Time.time - startTime < timeLimit)
        {

            if (Input.GetKeyDown(KeyCode.Z))
            {
                hitCount++;
                Debug.Log($"��Ÿ Ƚ��: {hitCount}/{requestHit}");

                progressBar.value = (float)hitCount / requestHit;
                UpdateProgressBarColor(progressBar.value);
            }

            if (hitCount >= requestHit)
            {
                Debug.Log("���Ҵ�!");
                fishInfo.ShowFishInfo(currentFish);

                progressBar.gameObject.SetActive(false);

                state = "playing";

                player.GetComponent<Player>().EnableMovement();

                yield break;
            }
            yield return null;
        }

        Debug.Log("�̳��� �԰� ��������...");
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
