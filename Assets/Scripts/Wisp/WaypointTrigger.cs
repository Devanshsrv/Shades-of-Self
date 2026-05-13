using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    public WispController wisp;
    public int index;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger fired! Index = " + index + " | Tag = " + col.tag);

        if (col.CompareTag("Player"))
        {
            wisp.playerWaypointIndex = index;
            Debug.Log("Assigned PlayerWaypointIndex = " + index);
        }
    }

}
