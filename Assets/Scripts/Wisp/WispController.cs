using UnityEngine;

public class WispController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float hoverOffset = 0.4f;
    public float hopHeight = 0.4f;
    public float hopSpeed = 2f;

    [Header("Path")]
    public Transform movePointsParent;
    private Transform[] movePoints;

    [Header("Player")]
    public Transform player;
    public int playerWaypointIndex = 0;

    // how far is considered "too far"
    public int forwardTolerance = 1;
    public int backwardTolerance = 1;

    private int wispWaypointIndex = 0;
    private bool isMoving = false;

    [Header("FX (optional)")]
    public Animator smokeAnimator;
    public float singleJumpHeight = 0.5f;   // adjust based on real player jump


    void Start()
    {
        int count = movePointsParent.childCount;
        movePoints = new Transform[count];

        for (int i = 0; i < count; i++)
            movePoints[i] = movePointsParent.GetChild(i);

        transform.position = movePoints[0].position + Vector3.up * hoverOffset;
    }

    void Update()
    {
        if (isMoving) return;

        int diff = playerWaypointIndex - wispWaypointIndex;

        // --------------------------------------------------------
        // 1. PLAYER REACHED CURRENT WISP POINT → MOVE FORWARD
        // --------------------------------------------------------
        if (diff == 0 && wispWaypointIndex < movePoints.Length - 1)
        {
            MoveToIndex(wispWaypointIndex + 1);
            return;
        }

        // --------------------------------------------------------
        // 2. PLAYER TOO FAR AHEAD → CATCH UP
        // Example: playerIndex = 7, wispIndex = 3 → diff = +4
        // --------------------------------------------------------
        if (diff >= forwardTolerance)
        {
            MoveToIndex(playerWaypointIndex);
            return;
        }

        // --------------------------------------------------------
        // 3. PLAYER TOO FAR BEHIND → WISP RETURNS
        // Example: playerIndex = 3, wispIndex = 8 → diff = -5
        // --------------------------------------------------------
        if (diff <= -backwardTolerance)
        {
            MoveToIndex(playerWaypointIndex);
            return;
        }

        // If difference is within tolerance → do nothing and wait
    }

    void MoveToIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= movePoints.Length) return;

        wispWaypointIndex = newIndex;
        Vector3 target = movePoints[newIndex].position + Vector3.up * hoverOffset;

        // SMOKE FX — detect if this movement requires a "double jump"-like hop
        float heightDiff = target.y - transform.position.y;
        if (smokeAnimator && heightDiff > singleJumpHeight)
        {
            smokeAnimator.SetTrigger("Play");
        }


        StartCoroutine(HopMoveRoutine(target));
    }

    System.Collections.IEnumerator HopMoveRoutine(Vector3 targetPos)
    {
        isMoving = true;

        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * hopSpeed;

            float smoothT = Mathf.SmoothStep(0, 1, t);
            float heightCurve = Mathf.Sin(smoothT * Mathf.PI) * hopHeight;

            transform.position =
                Vector3.Lerp(start, targetPos, smoothT)
                + Vector3.up * heightCurve;

            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}
