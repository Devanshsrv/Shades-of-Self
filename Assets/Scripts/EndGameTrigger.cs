using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (triggered) return;

        if (col.CompareTag("Player"))
        {
            triggered = true;
            FindObjectOfType<EndScreenUI>()?.ShowEndScreen();
        }
    }
}
