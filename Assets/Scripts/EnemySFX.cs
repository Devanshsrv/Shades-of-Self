using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    public AudioSource source;

    [Header("Sword Enemy (Enemy 1)")]
    public AudioClip[] swordWooshClips;   // assign only for sword variant

    [Header("Punch Enemy (Enemy 2)")]
    public AudioClip heavyPunchClip;      // assign only for punch variant

    // Called when attack animation starts
    public void PlayAttackSound()
    {
        // If we have sword wooshes → random swoosh (Enemy 1)
        if (swordWooshClips != null && swordWooshClips.Length > 0)
        {
            int i = Random.Range(0, swordWooshClips.Length);
            AudioClip clip = swordWooshClips[i];
            if (clip != null)
                source.PlayOneShot(clip);
        }
        // Otherwise if we have heavy punch → play that (Enemy 2)
        else if (heavyPunchClip != null)
        {
            source.PlayOneShot(heavyPunchClip);
        }
    }
}
