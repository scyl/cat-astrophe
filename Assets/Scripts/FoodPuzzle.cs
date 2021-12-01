using BansheeGz.BGSpline.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPuzzle : MonoBehaviour
{
    public bool complete;
    // How much distance to move in the real world.
    public float walkingSpeed = 0.03f;
    public float leaveDeskDelay = 0.5f;

    public List<GameObject> tableBlockers;
    public GoalController foodTrigger;
    public GameObject television;
    public BGCcMath curve;

    // Start off in the chair. For `cursor.DistanceRatio`:
    // - 0 is at the chair
    // - 1 is at the light switch
    private BGCcCursor cursor;

    // How many frames left we have to wait before we can leave the desk to turn off the light.
    private float leaveDeskDelayLeft = 0;

    // States:
    // - Going to (or at) light. In this state if light is on.
    //   May need to until `leaveDeskDelayFramesLeft` is zero.
    // - Going to (or at) desk. In this state if light is off
    //   Sets `leaveDeskDelayFramesLeft` upon reaching the desk.

    // Start is called before the first frame update
    void Start()
    {
        cursor = curve.GetComponent<BGCcCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (television.activeSelf)
        {
            if (0 < leaveDeskDelayLeft)
            {
                leaveDeskDelayLeft -= Time.deltaTime;
            }
            else
            {
                // move to turn off light
                cursor.Distance += walkingSpeed;
                tableBlockers.ForEach(blocker => blocker.SetActive(false));

                if (1 <= cursor.DistanceRatio)
                {
                    // at the remote
                    television.SetActive(false); // FIXME i think the human is meant to just watch tv?
                }
            }
        }
        else
        {
            // move back to chair
            cursor.Distance -= walkingSpeed;

            if (cursor.DistanceRatio <= 0)
            {
                // at the desk
                tableBlockers.ForEach(blocker => blocker.SetActive(true));
                leaveDeskDelayLeft = leaveDeskDelay;
            }
        }
    }
    private void FixedUpdate()
    {
        if (foodTrigger.enabled && foodTrigger.complete)
        {
            foodTrigger.enabled = false;
            complete = true;
        }
    }
}