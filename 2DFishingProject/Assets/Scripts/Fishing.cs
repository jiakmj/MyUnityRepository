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
    public Slider progressBar; //��������
    private Image progressBarFill;

    public GameObject progressBarPosition;

    public int requestHit = 10; //��Ÿ Ƚ��
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
                Debug.LogError("fishinfo ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }   

    public void StartFishing()
    {
        Debug.Log("Ŭ�� ���� ���� ����");

        if (currentFish != null)
        {
            Debug.Log($"���� �����: {currentFish.name}");
            StartCatching(currentFish);
        }
        else
        {
            Debug.Log("���� ����Ⱑ �������� �ʾҽ��ϴ�.");
            StartCoroutine(FishingProcess());
        }

        //fishAppearTime = Time.time + Random.Range(2f, 5f); //���� �ð�
        //StartCoroutine(FishingProcess());
    }

    IEnumerator FishingProcess()
    {
        float fishAppearTime = Random.Range(2f, 10f); //���� �ð�

        currentFish = fishManager.GetRandomFish(); //JSON���� Fish ���� �ҷ���       

        if (currentFish != null)
        {
            StartCatching(currentFish);
            Debug.Log($"�����: {currentFish.name}");
        }
        else
        {
            Debug.Log("����⸦ �������µ� �����߽��ϴ�.");
        }

        yield return new WaitForSeconds(fishAppearTime);
    }
    public void StartCatching(Fish fish)
    {
        Debug.Log("startCatching ȣ��");
        if (fish == null)
        {
            Debug.LogError("���޵� fish�� null�Դϴ�.");
            return;
        }        
        //isCatch = true;
        currentFish = fish;
        hitCount = 0;
        startTime = Time.time;
        progressBar.value = 0.0f;
        progressBar.gameObject.SetActive(true);
        //requestHit = fish.level * 5; //���̵� 1~5������ ����
        fishInfo.ShowFishInfo(currentFish);
        StartCoroutine(CatchingFishProcess());
        
        if (fishInfo != null)
        {
            fishInfo.ShowFishInfo(currentFish);
        }
        else
        {
            Debug.Log("fishinfo�� null�Դϴ�.");
        }

    }
    IEnumerator CatchingFishProcess()
    {
        Debug.Log("catchingfishprocess ����");
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
                yield break;
            }
            yield return null;
        }
        Debug.Log("�̳��� �԰� ��������...");
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
