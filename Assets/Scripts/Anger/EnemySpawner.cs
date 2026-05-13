using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyVariants;
    public GameObject enemyPrefab;

    public BossHealthBarUI bossHealthBar;
    public EnemyHealthBar enemyHealthBar;

    public GameObject levelExitTrigger;   // 🔹 NEW: assign in inspector

    private GameObject currentEnemy;

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (currentEnemy != null)
            return;

        int rand = Random.Range(0, spawnPoints.Length);
        currentEnemy = Instantiate(
            enemyPrefab,
            spawnPoints[rand].position,
            Quaternion.identity
        );

        currentEnemy.layer = LayerMask.NameToLayer("Enemy");

        EnemyAI ai = currentEnemy.GetComponent<EnemyAI>();
        EnemyHealth health = currentEnemy.GetComponent<EnemyHealth>();

        ai.teleportPoints = spawnPoints;
        ai.enemyVariants = enemyVariants;

        // 🔹 assign exit trigger to AI at runtime
        ai.levelExitTrigger = levelExitTrigger;

        // initialize health bars
        health.InitializeHealth(bossHealthBar, enemyHealthBar);
    }
}
