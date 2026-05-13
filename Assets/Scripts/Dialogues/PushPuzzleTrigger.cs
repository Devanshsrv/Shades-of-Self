using UnityEngine;

public class PushPuzzleTrigger : MonoBehaviour
{
    public SimpleNPCDialogue npc;

    bool solved = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (solved) return;

        if (col.CompareTag("PushBox"))
        {
            solved = true;

            // Mark task as completed
            npc.MarkTaskDone();

            Debug.Log("Push Puzzle Solved!");

            Destroy(gameObject); // optional, remove trigger
        }
    }
}
