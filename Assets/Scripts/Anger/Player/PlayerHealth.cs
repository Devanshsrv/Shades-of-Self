using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private PlayerHealthBarUI healthBar;

    public bool healthEnabled = false;   // <-- Only true in Anger Level

    public void Initialize(PlayerHealthBarUI bar, bool enabledInLevel)
    {
        healthEnabled = enabledInLevel;
        healthBar = bar;

        if (!healthEnabled)
        {
            // Hide UI if not in anger level
            if (healthBar != null)
                healthBar.gameObject.SetActive(false);
            return;
        }

        // Show + initialize UI
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.SetMax(maxHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        if (!healthEnabled) return; // No damage outside anger level

        currentHealth -= amount;
        SFXManager.Instance.Play(SFXManager.Instance.playerHit);

        if (healthBar != null)
            healthBar.SetValue(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Player died.");

        // Restart the current level
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
