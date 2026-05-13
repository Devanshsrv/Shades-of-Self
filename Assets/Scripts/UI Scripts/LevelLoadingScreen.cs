using UnityEngine;
using TMPro;
using System.Collections;

public class LevelLoadingScreen : MonoBehaviour
{
    public string levelTitle;
    [TextArea] public string levelDescription;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI pressEnterText;

    void Start()
    {
        Time.timeScale = 0f; // freeze gameplay

        titleText.text = levelTitle;
        descriptionText.text = levelDescription;

        pressEnterText.alpha = 0f;

        // Blink hint
        StartCoroutine(BlinkPressEnter());
    }

    IEnumerator BlinkPressEnter()
    {
        yield return new WaitForSecondsRealtime(2f); // wait before showing

        while (true)
        {
            pressEnterText.alpha = Mathf.PingPong(Time.unscaledTime * 1.5f, 1f);
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CloseScreen();
        }
    }

    void CloseScreen()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
