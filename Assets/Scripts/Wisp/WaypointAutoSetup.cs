using UnityEngine;

public class WaypointAutoSetup : MonoBehaviour
{
    public WispController wisp;
    public float triggerSize = 0.5f;

    public void Start()
    {
    }

    public void RunSetup()
{
    if (wisp == null)
    {
        Debug.LogError("WaypointAutoSetup: NO WISP assigned!");
        return;
    }

    int index = 0;

    foreach (Transform child in transform)
    {
        BoxCollider2D col = child.gameObject.GetComponent<BoxCollider2D>();
        if (col == null)
            col = child.gameObject.AddComponent<BoxCollider2D>();

        col.isTrigger = true;
        col.size = new Vector2(triggerSize, triggerSize);

        WaypointTrigger wt = child.gameObject.GetComponent<WaypointTrigger>();
        if (wt == null)
            wt = child.gameObject.AddComponent<WaypointTrigger>();

        wt.wisp = wisp;
        wt.index = index;

        index++;
    }

    Debug.Log("WaypointAutoSetup complete. Assigned " + index + " triggers.");
}

}
