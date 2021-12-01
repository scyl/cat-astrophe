using BansheeGz.BGSpline.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaController : MonoBehaviour
{
    public BGCcMath curve;
    private BGCcCursor cursor;
    public bool isRunning;
    private bool goingForward = true;
    public float leaveDockDelay = 0.5f, walkingSpeed = 0.03f;
    private float leaveDockDelayLeft;
    internal AudioSource audioSource;
    // we'll jump to this point in the audio when the roomba stops. should be whenever the sound starts to die down
    public float vacuumClipEnd;

    void Start()
    {
        cursor = curve.GetComponent<BGCcCursor>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isRunning)
        {
            if (audioSource.time > vacuumClipEnd - 5)
            {
                // uh oh, jump back a bit
                audioSource.time -= 5;
            }
            if (goingForward)
            {
                if (0 < leaveDockDelayLeft)
                {
                    leaveDockDelayLeft -= Time.deltaTime;
                }
                else
                {

                    var before = cursor.CalculatePosition();
                    cursor.Distance += walkingSpeed;
                    var after = cursor.CalculatePosition();

                    var diff = after - before;
                    if (playerIsRiding)
                    {
                        GameController.globalController.player.GetComponent<CharacterController>().Move(diff);
                    }


                    if (1 <= cursor.DistanceRatio)
                    {
                        // goingForward = false;
                        leaveDockDelayLeft = leaveDockDelay;
                        isRunning = false;
                        goingForward = true;
                        cursor.Distance = 0;
                    }
                }
            }
            else
            {
                // go backwards through path
                cursor.Distance -= walkingSpeed;

                if (cursor.DistanceRatio <= 0)
                {
                    leaveDockDelayLeft = leaveDockDelay;
                    isRunning = false;
                    audioSource.time = vacuumClipEnd;
                    goingForward = true;
                }
            }
        }
    }

    public bool complete;
    private float lastCollision = -100f;
    public bool playerIsRiding = false;
    public float totalRideTime;
    private void FixedUpdate()
    {
        playerIsRiding = Time.time - lastCollision < 0.05f;

        if (playerIsRiding)
        {
            totalRideTime += Time.deltaTime;

            complete = true;
        }
    }

    private void PlayerHit(ControllerColliderHit hit)
    {
        if (hit.normal.normalized.y > 0.2 && isRunning)
        {
            // moving downwards, start riding?
            lastCollision = Time.time;
        }
    }

    public void StartRunning()
    {
        if (!isRunning)
        {
            isRunning = true;
            audioSource.Play();
        }
    }
}
