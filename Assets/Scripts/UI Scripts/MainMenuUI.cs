using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    public Image fadeImage;
    public float fadeTime = 1f;

    void Start()
    {
        // Fade in
        //StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        Debug.Log("PLAY BUTTON PRESSED");   // ← add this
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        Time.timeScale = 1f;
        Debug.Log("LOADGAME STARTED");
        fadeImage.gameObject.SetActive(true);

        Color c = fadeImage.color;
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            Debug.Log("FADING t=" + t);
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }
        Debug.Log("LOADING SCENE NOW");
        SceneManager.LoadScene("Starting Level");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Pressed");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
