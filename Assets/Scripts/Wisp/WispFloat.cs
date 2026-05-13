using UnityEngine;

public class WispFloat : MonoBehaviour
{
    public float amplitude = 0.1f;   // how much it floats up/down
    public float frequency = 2f;     // how fast it floats

    private Vector3 baseOffset;

    void Start()
    {
        // store local offset so hover follows movement
        baseOffset = transform.localPosition;
    }

    void Update()
    {
        Vector3 pos = baseOffset;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = pos;
    }
}
