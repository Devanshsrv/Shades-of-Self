using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Scene Order")]
    public string[] levelNames;   // assign level order here

    private int currentIndex = 0;

    [Header("UI References")]
    public PlayerHealthBarUI playerHealthBar;

    void Start()
    {
        // identify current scene's index
        string currentScene = SceneManager.GetActiveScene().name;

        for (int i = 0; i < levelNames.Length; i++)
        {
            if (levelNames[i] == currentScene)
            {
                currentIndex = i;
                break;
            }
        }

        SetupPlayerForThisLevel();
    }

    void SetupPlayerForThisLevel()
    {
        // find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;

        PlayerHealth pHealth = playerObj.GetComponent<PlayerHealth>();
        PlayerMovement pMove = playerObj.GetComponent<PlayerMovement>();

        bool isAngerLevel = SceneManager.GetActiveScene().name == "Anger";

        // 1. Enable or disable combat based on level
        if (pMove != null)
            pMove.combatEnabled = isAngerLevel;

        // 2. Initialize Player Health UI only in Anger Level
        if (pHealth != null)
            pHealth.Initialize(playerHealthBar, isAngerLevel);

        // 3. Toggle player health bar visibility
        if (playerHealthBar != null)
            playerHealthBar.gameObject.SetActive(isAngerLevel);
    }

    public void LoadNextLevel()
    {
        int next = currentIndex + 1;

        if (next < levelNames.Length)
            SceneManager.LoadScene(levelNames[next]);
        else
            Debug.Log("GAME COMPLETE!");
    }
}
