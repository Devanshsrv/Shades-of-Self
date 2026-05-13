using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    public SimpleNPCDialogue npc;   // Reference to the NPC that needs this coin

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Player collected coin!");

            npc.MarkCollected();     // tells NPC the quest is done

            Destroy(gameObject);    // remove the coin
        }
    }
}
