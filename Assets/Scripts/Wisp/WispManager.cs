using UnityEngine;

public class WispManager : MonoBehaviour
{
    public GameObject wispPrefab;
    public Transform jumpPointsParent;
    public Transform player;

    void Start()
    {
        Transform firstPoint = jumpPointsParent.GetChild(0);

        GameObject wisp = Instantiate(
            wispPrefab,
            firstPoint.position,
            Quaternion.identity
        );

        WispController controller = wisp.GetComponent<WispController>();
        controller.player = player;                 // NOW VALID
        controller.movePointsParent = jumpPointsParent;
    }
}
