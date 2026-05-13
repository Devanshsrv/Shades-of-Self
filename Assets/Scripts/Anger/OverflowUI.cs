using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class OverflowUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fadeScreen;            // full black image
    public TextMeshProUGUI messageText; // TMP text
    public Button restartButton;        // normal UI button

    public float fadeDuration = 1f;
    public float textFadeDuration = 1f;
    private bool sequenceStarted = false;

    void Start()
    {
        // Make sure elements start hidden
        Color c = fadeScreen.color;
        c.a = 0;
        fadeScreen.color = c;

        Color t = messageText.color;
        t.a = 0;
        messageText.color = t;

        restartButton.gameObject.SetActive(false);
    }

    public void StartOverflowSequence()
{
    if (sequenceStarted) return;   // <-- prevents double run
    sequenceStarted = true;

    StartCoroutine(FadeSequence());
}

    IEnumerator FadeSequence()
    {
        // ======================
        // 1. Fade screen to black
        // ======================
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;

            Color c = fadeScreen.color;
            c.a = alpha;
            fadeScreen.color = c;

            yield return null;
        }

        // Ensure full opaque
        fadeScreen.color = new Color(0, 0, 0, 1f);

        // ======================
        // 2. Fade in TMP text
        // ======================
        for (float t = 0; t < textFadeDuration; t += Time.deltaTime)
        {
            float alpha = t / textFadeDuration;

            Color tc = messageText.color;
            tc.a = alpha;
            messageText.color = tc;

            yield return null;
        }

        // ensure fully visible
        Color fullText = messageText.color;
        fullText.a = 1f;
        messageText.color = fullText;

        // ======================
        // 3. Enable Restart button
        // ======================
        restartButton.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
