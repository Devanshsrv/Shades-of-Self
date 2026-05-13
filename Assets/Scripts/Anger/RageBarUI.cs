using UnityEngine;
using UnityEngine.UI;

public class RageBarUI : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    [Header("Colors")]
    public Color calmColor = Color.green;
    public Color unstableColor = new Color(1f, 0.6f, 0f); // orange
    public Color overflowColor = Color.red;

    [Header("Shake")]
    public RectTransform barRoot;
    public float shakeIntensity = 5f;

    private float targetValue = 0f;

    [Header("Player FX")]
    public SpriteRenderer playerSprite;
    public Color normalColor = Color.white;
    public Color rageColor = new Color(1f, 0.3f, 0.3f);
    public float blinkThreshold = 0.90f;
    public float blinkSpeed = 10f;

    void Update()
    {
        // Smooth UI fill
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * 8f);
        //Debug.Log("slider=" + slider.value + " target=" + targetValue);

        // Shake only if unstable (80–90)
        if (slider.value >= 80 && slider.value < 90)
        {
            float x = Mathf.Sin(Time.time * 40f) * shakeIntensity;
            barRoot.anchoredPosition = new Vector2(x, barRoot.anchoredPosition.y);
        }
        else
        {
            // return to normal
            barRoot.anchoredPosition = Vector2.zero;
        }
    }

    // Called by RageManager
    public void UpdateRage(float value)
    {
        targetValue = value;   // ← smooth interpolation target
        //Debug.Log("UPDATE RAGE CALLED");

        // -------------------------
        // 1. Update the UI bar color
        // -------------------------
        if (value < 80f)
            fill.color = calmColor;
        else if (value < 90f)
            fill.color = unstableColor;
        else
            fill.color = overflowColor;

        // -------------------------
        // 2. Update player FX color
        // -------------------------
        float v = value / 100f;  // normalize 0–1 for color lerp

        if (v < 0.90f)
        {
            // Normal red intensity based on rage
            playerSprite.color = Color.Lerp(normalColor, rageColor, v);
        }
        else
        {
            // High-rage blinking effect
            float blink = Mathf.Abs(Mathf.Sin(Time.time * 10f));
            playerSprite.color = Color.Lerp(rageColor, Color.white, blink);
        }
    }

}
