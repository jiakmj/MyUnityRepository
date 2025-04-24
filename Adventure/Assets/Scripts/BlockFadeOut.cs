using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockFadeOut : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void StartFade()
    {
        StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeOutAndDisable()
    {
        float elapsed = 0f;
        Color originalColor = tilemap.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        SoundManager.Instance.PlaySFX(SFXType.DestroyTile);

        tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        gameObject.SetActive(false); // ¶Ç´Â Destroy(gameObject);
    }
}
