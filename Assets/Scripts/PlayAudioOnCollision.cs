using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnCollision : MonoBehaviour
{
    public AudioClip clip;
    internal AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2.0 && GameController.globalController.player.controlEnabled)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
