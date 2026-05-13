using UnityEngine;

public class AutoSetupWispPoints : MonoBehaviour
{
    public WispController wisp;
    public float triggerSize = 0.5f;

    void Awake()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform point = transform.GetChild(i);

            // Add collider
            BoxCollider2D col = point.gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(triggerSize, triggerSize);

            // Add trigger script
           /* WispTrigger trigger = point.gameObject.AddComponent<WispTrigger>();
            trigger.wisp = wisp;*/
        }
    }
}
