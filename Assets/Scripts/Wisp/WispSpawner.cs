using UnityEngine;

public class WispSpawner : MonoBehaviour
{
    public Transform jumpPointsParent;
    public Transform player;

    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Wisp");
        GameObject wisp = Instantiate(prefab);

        WispController controller = wisp.GetComponent<WispController>();
        controller.movePointsParent = jumpPointsParent;
        controller.player = player;

        // Critical assignment
        WaypointAutoSetup setup = jumpPointsParent.GetComponent<WaypointAutoSetup>();
        setup.wisp = controller;

        // Now safely run the AutoSetup
        setup.RunSetup();
    }
}
