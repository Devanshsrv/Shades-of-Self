using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RageManager : MonoBehaviour
{
    [Header("Rage Values")]
    [Range(0, 100)]
    public float rage = 0f;

    public float rageGainPerMiss = 12f;
    public float rageDecayRate = 6f;

    [Header("Rage Thresholds")]
    public float unstableMin = 80f;
    public float overflowMin = 90f;

    [Header("Events")]
    public UnityEvent OnEnterUnstable;
    public UnityEvent OnExitUnstable;

    public UnityEvent OnEnterOverflow;
    public UnityEvent OnExitOverflow;

    public UnityEvent<float> OnRageChanged;

    private bool inUnstable = false;
    private bool inOverflow = false;

    [Header("Player FX")]
    public SpriteRenderer playerSprite;
    public Color normalColor = Color.white;
    public Color rageColor = new Color(1f, 0.3f, 0.3f);

    private bool uiTriggered = false;


    void Update()
    {
        // 🟢 RAGE DECAY (ONLY WHEN NOT IN OVERFLOW)
        if (!inOverflow)
        {
            if (rage > 0)
            {
                rage -= rageDecayRate * Time.deltaTime;
                rage = Mathf.Clamp(rage, 0, 100);

                // 🔵 UPDATE UI
                OnRageChanged?.Invoke(rage);
            }
        }

        // 🟣 CHECK STATE TRANSITIONS
        UpdateState();
    }


    // 🟡 MISS = RAGE GAIN
    public void AddRageFromMiss()
    {
        if (inOverflow) return; // cannot increase beyond overflow logic

        rage += rageGainPerMiss;
        rage = Mathf.Clamp(rage, 0, 100);

        // 🔵 Update UI when rage changes
        OnRageChanged?.Invoke(rage);

        UpdateState();
    }


    private void UpdateState()
    {
        // ===========================================================
        // 🟧 UNSTABLE STATE (80–90%)
        // ===========================================================
        if (!inOverflow && rage >= unstableMin && rage < overflowMin)
        {
            if (!inUnstable)
            {
                inUnstable = true;
                OnEnterUnstable?.Invoke();     // event triggered
            }
        }
        else
        {
            if (inUnstable)
            {
                inUnstable = false;
                OnExitUnstable?.Invoke();      // event triggered
            }
        }


        // ===========================================================
        // 🔴 OVERFLOW STATE (≥ 90%)
        // ===========================================================
if (rage >= overflowMin)
{
    if (!inOverflow)
    {
        inOverflow = true;
        OnEnterOverflow?.Invoke();

        // Player enters overflow mode
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm != null)
            pm.EnableOverflowMode();

        // Enemy freezes
        EnemyAI enemy = FindObjectOfType<EnemyAI>();
        if (enemy != null)
            enemy.FreezeEnemy();

        if (!uiTriggered)
        {
            uiTriggered = true;
            StartCoroutine(DelayedOverflowUI());
        }
    }
}

    }

   private IEnumerator DelayedOverflowUI()
{
    // Wait 3 seconds BEFORE fade begins
    yield return new WaitForSeconds(15f);

    OverflowUI ui = FindObjectOfType<OverflowUI>();
    if (ui != null)
        ui.StartOverflowSequence();
}


}
