using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioController : MonoBehaviour
{
    internal AudioSource audioSource;
    public List<AudioClip> meow;
    private float lastMeowEnd;
    public AudioClip swipe;
    private float lastSwipeEnd;
    public AudioClip flush;
    private float lastFlushEnd;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public AudioClip collisionSound;
    private void Start()
    {
        // find all rigid bodies, attach something that plays the collision sound to them on collision, hope for the best
        foreach (var c in FindObjectsOfType<Rigidbody>())
        {
            var source = c.gameObject.AddComponent<AudioSource>();
            source.spatialize = true;
            source.spatialBlend = 0.5f;
            var cPlayer = c.gameObject.AddComponent<PlayAudioOnCollision>();
            cPlayer.clip = collisionSound;
        }
    }

    public void Meow()
    {
        if (lastMeowEnd < Time.time)
        {
            var picked = meow[Random.Range(0, meow.Count)];
            audioSource.PlayOneShot(picked);
            lastMeowEnd = Time.time + picked.length;
        }
    }
    public void Swipe()
    {
        if (lastSwipeEnd < Time.time)
        {
            audioSource.PlayOneShot(swipe);
            lastSwipeEnd = Time.time + swipe.length;
        }
    }
    public void Flush()
    {
        if (lastFlushEnd < Time.time)
        {
            audioSource.PlayOneShot(flush);
            lastFlushEnd = Time.time + flush.length;
        }
    }
}
