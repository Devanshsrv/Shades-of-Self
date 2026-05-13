using UnityEngine;
using TMPro;

public class SimpleDialogueUI : MonoBehaviour
{
    public static SimpleDialogueUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject panel;              // your DialoguePanel
    public TextMeshProUGUI nameText;      // NameText
    public TextMeshProUGUI bodyText;      // DialogueText
    public TextMeshProUGUI choicesText;   // ChoicesText (optional)

    public bool IsOpen => panel.activeSelf;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        HideAll();
    }

    // Show a single line (no choices)
    public void ShowLine(string speaker, string text)
    {
        panel.SetActive(true);

        if (nameText != null)
            nameText.text = speaker;

        if (bodyText != null)
            bodyText.text = text;

        if (choicesText != null)
            choicesText.text = "";
    }

    // Show choices (for type 3 NPC)
    public void ShowChoices(string[] options)
    {
        if (choicesText == null) return;

        if (options == null || options.Length == 0)
        {
            choicesText.text = "";
            return;
        }

        string formatted = "";
        for (int i = 0; i < options.Length; i++)
        {
            formatted += (i + 1) + ". " + options[i] + "\n";
        }

        choicesText.text = formatted;
    }

    public void HideAll()
    {
        panel.SetActive(false);

        if (bodyText != null) bodyText.text = "";
        if (nameText != null) nameText.text = "";
        if (choicesText != null) choicesText.text = "";
    }
}
