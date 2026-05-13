using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public EnemyHealthBar healthBar;
    public EnemyAI enemyAI;
    public BossHealthBarUI bossBar;


    public void InitializeHealth(BossHealthBarUI bossUI, EnemyHealthBar enemyUI)
    {
        bossBar = bossUI;
        healthBar = enemyUI;

        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        if (bossBar != null)
            bossBar.SetMax(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        AudioManager.Instance.UpdateEnemyHealth(currentHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (bossBar != null)
            bossBar.SetValue(currentHealth);

        if (enemyAI != null)
            enemyAI.PlayHit();

        if (currentHealth <= 0)
            enemyAI.Die();
    }
}
