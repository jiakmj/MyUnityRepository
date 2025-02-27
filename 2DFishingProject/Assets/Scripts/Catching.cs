using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Catching : MonoBehaviour
{
    //public int requestHit = 10; //ø¨≈∏ »Ωºˆ
    //private int hitCount = 0;
    //private float timeLimit = 3.0f;
    //private float startTime;
    ////private bool isCatch = false;
    //public Slider progressBar; //∞‘¿Ã¡ˆπŸ
    
    //private Image progressBarFill;
    //public GameObject player;
    //public Fish currentFish;
    //public FishInfo FishInfo;
    //public GameObject progressBarPosition;

    //void Update()
    //{
    //    if (isCatch)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            hitCount++;
    //            Debug.Log($"ø¨≈∏ »Ωºˆ: {hitCount}/{requestHit}");
    //        }

    //        if (hitCount >= requestHit)
    //        {
    //            Debug.Log("≥¨æ“¥Ÿ!");
    //            isCatch = false;
    //            FishInfo.ShowFishInfo(currentFish);
    //        }

    //        if (Time.time - startTime > timeLimit)
    //        {
    //            Debug.Log("πÃ≥¢∏∏ ∏‘∞Ì µµ∏¡∞¨¥Ÿ...");
    //            isCatch = false;
    //            FishInfo.HideFishInfo();
    //        }
    //    }
    //}
    
    //void Start()
    //{
    //    progressBar.transform.SetParent(progressBarPosition.transform);
    //    progressBar.transform.localPosition = Vector3.zero;
    //    progressBar.gameObject.SetActive(false);         
    //}


    //public void StartCatching(Fish fish)
    //{
    //    Debug.Log("startChatcing »£√‚");
    //    //isCatch = true;
    //    currentFish = fish;
    //    hitCount = 0;
    //    startTime = Time.time;
    //    progressBar.value = 0.0f;
    //    progressBar.gameObject.SetActive(true);
    //    //requestHit = fish.level * 5; //≥≠¿Ãµµ 1~5¡§µµ∑Œ º¬∆√
    //    FishInfo.ShowFishInfo(currentFish);
    //    StartCoroutine(CatchingFishProcess());
    //}

    //IEnumerator CatchingFishProcess()
    //{
    //    Debug.Log("catchingfishprocess Ω√¿€");
    //    while (Time.time - startTime < timeLimit)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            hitCount++;
    //            Debug.Log($"ø¨≈∏ »Ωºˆ: {hitCount}/{requestHit}");

    //            progressBar.value = (float)hitCount/requestHit;
    //            UpdateProgressBarColor(progressBar.value);
    //        }

    //        if (hitCount >= requestHit)
    //        {
    //            Debug.Log("≥¨æ“¥Ÿ!");
    //            FishInfo.ShowFishInfo(currentFish);
    //            progressBar.gameObject.SetActive(false);
    //            yield break;
    //        }
    //        yield return null;
    //    }        
    //    Debug.Log("πÃ≥¢∏∏ ∏‘∞Ì µµ∏¡∞¨¥Ÿ...");
    //    FishInfo.HideFishInfo();
    //    progressBar.gameObject.SetActive(false);
    //}
    //private void UpdateProgressBarColor(float value)
    //{
    //    if (value < 0.5f)
    //    {
    //        progressBarFill.color = Color.black;
    //    }
    //    else if (value < 0.8f)
    //    {
    //        progressBarFill.color = new Color(1.0f, 0.5f, 0.0f);
    //    }
    //    else
    //    {
    //        progressBarFill.color = Color.red;
    //    }
    //}
}
