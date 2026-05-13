using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource source;

    [Header("Player SFX")]
    public AudioClip punch1;          // light punch
    public AudioClip punch2;          // heavy punch
    public AudioClip playerHit;

    public AudioClip jump;            // <- NEW
    public AudioClip doubleJump;      // <- NEW

    [Header("Enemy SFX (Randomized Woosh)")]
    public AudioClip[] enemyWooshClips;   // <- NOW AN ARRAY
    public AudioClip enemyPunch;          // heavy impact

    void Awake()
    {
        Instance = this;
    }

    // --- generic player ---
    public void Play(AudioClip clip)
    {
        if (clip != null)
            source.PlayOneShot(clip);
    }

    // --- RANDOM PICK ---
    public void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        int i = Random.Range(0, clips.Length);
        AudioClip clip = clips[i];

        if (clip != null)
            source.PlayOneShot(clip);
    }
}
