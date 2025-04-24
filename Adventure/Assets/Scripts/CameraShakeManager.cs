using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalPos;

    [Header("카메라 쉐이크 설정")]
    public CinemachineImpulseSource impulseSource;

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

    private void Start()
    {
        if (Camera.main != null)
        {
            originalPos = Camera.main.transform.localPosition;
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        originalPos = Camera.main.transform.localPosition;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        if (Camera.main == null)
        {
            yield return null;
        }

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, originalPos.y + y, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }

    public void GenerateCameraImpulse()
    {
        if (impulseSource != null)
        {
            Debug.Log("카메라 임펄스 발생");
            impulseSource.GenerateImpulse();
        }
        else

        {
            Debug.LogWarning("ImpulseSource가 연결이 안 되어있습니다.");
        }
    }

}
