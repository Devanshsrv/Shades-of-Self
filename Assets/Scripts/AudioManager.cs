using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Main Menu Music")]
    public AudioClip mainMenuMusic;

    [Header("Level Music (Normal Levels)")]
    public AudioClip startingLevelMusic;   // Starting Level
    public AudioClip denialMusic;          // Denial
    public AudioClip guiltMusic;           // Guilt
    public AudioClip lonelinessMusic;      // Loneliness
    public AudioClip hopeMusic;            // Hope (after Anger)

    [Header("Anger Level Special Music")]
    public AudioClip angerHighHealthMusic;  // enemy health > 70
    public AudioClip angerLowHealthMusic;   // enemy health < 70

    private AudioSource audioSource;

    private bool isAngerLevel = false;
    private float currentEnemyHealth = 100f;

    private string lastSceneName = "";

    private void Awake()
    {
        // SINGLETON: destroy duplicates
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;

        // Handle scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // 🔥 Ensures music plays when starting directly in ANY level (Unity editor)
        string sceneName = SceneManager.GetActiveScene().name;
        PlayMusicForScene(sceneName);

        lastSceneName = sceneName;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        isAngerLevel = sceneName == "Anger";

        // 🔥 Detect scene restart (same scene reloaded)
        if (sceneName == lastSceneName)
        {
            RestartLevelMusic(sceneName);
        }
        else
        {
            PlayMusicForScene(sceneName);
        }

        lastSceneName = sceneName;
    }

    // 🔥 Plays correct music for ANY scene
    public void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "Main Menu")
        {
            RestartClip(mainMenuMusic);
            return;
        }

        isAngerLevel = sceneName == "Anger";

        if (!isAngerLevel)
        {
            RestartClip(GetNormalLevelClip(sceneName));
        }
        else
        {
            RestartClip(currentEnemyHealth > 70 ? angerHighHealthMusic : angerLowHealthMusic);
        }
    }

    // 🔥 When the SAME level reloads → restart music
    private void RestartLevelMusic(string sceneName)
    {
        PlayMusicForScene(sceneName);
    }

    // Normal levels
    private AudioClip GetNormalLevelClip(string sceneName)
    {
        switch (sceneName)
        {
            case "Starting Level": return startingLevelMusic;
            case "Denial": return denialMusic;
            case "Guilt": return guiltMusic;
            case "Lonliness": return lonelinessMusic;
            case "Hope": return hopeMusic;
        }

        return null;
    }

    // 🔥 Anger level enemy health change
    public void UpdateEnemyHealth(float health)
    {
        if (!isAngerLevel) return;

        currentEnemyHealth = health;

        RestartClip(currentEnemyHealth > 70 ? angerHighHealthMusic : angerLowHealthMusic);
    }

    // 🔥 Always restarts clip from start
    private void RestartClip(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.time = 0f;
        audioSource.Play();
    }
}
