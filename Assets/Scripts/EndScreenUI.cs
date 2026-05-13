using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class EndScreenUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image background;               // plain image
    public TextMeshProUGUI messageText;    // main message
    public TextMeshProUGUI byLineText;     // developed by...
    public TextMeshProUGUI pressEnterText; // blinking press enter

    [Header("Texts")]
    [TextArea] public string finalMessage;
    public string byLine = "— Developed by Devansh Srivastava";

    public float fadeTime = 1f;

    private bool canExit = false;

    void Start()
    {
        // Start hidden
        SetAlpha(background, 0f);
        SetAlpha(messageText, 0f);
        SetAlpha(byLineText, 0f);
        SetAlpha(pressEnterText, 0f);

        pressEnterText.text = "Be Happy...";
    }

    public void ShowEndScreen()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        Time.timeScale = 0f; // pause gameplay completely

        // Fade in background
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            float a = t / fadeTime;
            SetAlpha(background, a);
            yield return null;
        }
        SetAlpha(background, 1f);

        // Fade in main message
        messageText.text = finalMessage;

        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            float a = t / fadeTime;
            SetAlpha(messageText, a);
            yield return null;
        }
        SetAlpha(messageText, 1f);

        // Fade in by-line
        byLineText.text = byLine;

        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            float a = t / fadeTime;
            SetAlpha(byLineText, a);
            yield return null;
        }
        SetAlpha(byLineText, 1f);

        // Blink press enter
        StartCoroutine(BlinkPressEnter());
        canExit = true;
    }

    IEnumerator BlinkPressEnter()
    {
        while (true)
        {
            float a = Mathf.PingPong(Time.unscaledTime * 1.5f, 1f);
            SetAlpha(pressEnterText, a);
            yield return null;
        }
    }

    void Update()
    {
        if (canExit && Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }

    // Utility to change alpha of Image
    void SetAlpha(Image img, float a)
    {
        Color c = img.color;
        c.a = a;
        img.color = c;
    }

    // Utility to change alpha of TMP text
    void SetAlpha(TextMeshProUGUI tmp, float a)
    {
        Color c = tmp.color;
        c.a = a;
        tmp.color = c;
    }
}
