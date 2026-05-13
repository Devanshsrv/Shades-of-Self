using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyAI ai;

    void Start()
    {
        // Find EnemyAI on parent (EnemyBoss root)
        ai = GetComponentInParent<EnemyAI>();
    }

    // This is what the Animation Event will call
    public void DealDamageEvent()
    {
        if (ai != null)
            ai.DealDamageEvent();
    }
}
