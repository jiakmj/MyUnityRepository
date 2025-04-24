using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroy : MonoBehaviour
{
    public Tilemap tilemap; // Inspector에 직접 지정
    public Vector3Int startCell;
    public Vector3Int endCell;
    public float delay = 0.1f;

    public AudioClip breakSound; // 효과음
    private AudioSource audioSource;

    private void Awake()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void DestroyTiles()
    {
        StartCoroutine(DestroyTilesRoutine());
    }

    private IEnumerator DestroyTilesRoutine()
    {
        for (int y = startCell.y; y <= endCell.y; y++)
        {
            for (int x = startCell.x; x <= endCell.x; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(pos))
                {
                    tilemap.SetTile(pos, null);

                    if (breakSound != null && audioSource != null)
                    {
                        SoundManager.Instance.PlaySFX(SFXType.DestroyTile);
                    }

                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}
