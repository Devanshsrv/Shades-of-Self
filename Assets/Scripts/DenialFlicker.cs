using UnityEngine;
using UnityEngine.Tilemaps;

public class DenialFlicker : MonoBehaviour
{
    [Header("Tilemap Renderers")]
    public TilemapRenderer perfectWorld;
    public TilemapRenderer realWorld;

    [Header("Flicker Settings")]
    public float minInterval = 0.4f;
    public float maxInterval = 1.2f;
    public float realVisibleDuration = 0.25f;

    private void Start()
    {
        // Ensure perfect world is active at start
        realWorld.enabled = false;
        perfectWorld.enabled = true;

        StartCoroutine(FlickerRoutine());
    }

    System.Collections.IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);

            // REAL WORLD ON — PERFECT WORLD OFF
            perfectWorld.enabled = false;
            realWorld.enabled = true;

            yield return new WaitForSeconds(realVisibleDuration);

            // PERFECT WORLD BACK — REAL WORLD OFF
            realWorld.enabled = false;
            perfectWorld.enabled = true;
        }
    }
}
