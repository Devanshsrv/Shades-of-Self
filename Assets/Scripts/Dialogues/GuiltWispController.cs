using UnityEngine;

public class GuiltWispController : MonoBehaviour
{
    public Transform player;            // auto-assigned or dragged
    public float followSpeed = 3f;

    public int totalTasks = 3;          // how many NPC quests
    private int tasksCompleted = 0;

    public Vector3 initialScale = new Vector3(3f, 3f, 1f);
    public Vector3 shrinkAmount = new Vector3(0.8f, 0.8f, 0.8f);

    public GameObject exitTrigger;      // disabled at start, enabled when done

    void Start()
    {
        transform.localScale = initialScale;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (exitTrigger != null)
            exitTrigger.SetActive(false);
    }

    void Update()
    {
        // Follow player smoothly
        transform.position = Vector3.Lerp(
            transform.position,
            player.position,
            followSpeed * Time.deltaTime
        );
    }

    public void ShrinkOnce()
    {
        tasksCompleted++;

        // Instantly shrink
        transform.localScale -= shrinkAmount;

        //Movement restores increasingly
        FindObjectOfType<PlayerMovement>().ReceiveHelp();

        // Clamp scale to avoid inverting
        transform.localScale = new Vector3(
            Mathf.Max(0.1f, transform.localScale.x),
            Mathf.Max(0.1f, transform.localScale.y),
            1f
        );

        if (tasksCompleted >= totalTasks)
        {
            // Final disappearance
            Destroy(gameObject);

            // Enable exit
            if (exitTrigger != null)
                exitTrigger.SetActive(true);
        }
    }
}
