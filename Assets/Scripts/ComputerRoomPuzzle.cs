using BansheeGz.BGSpline.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerRoomPuzzle : MonoBehaviour
{
    public bool complete;
    // How much distance to move in the real world.
    public float walkingSpeed = 0.03f;
    public float leaveDeskDelay = 0.5f;

    public List<GameObject> tableBlockers;
    public GoalController keyboardTrigger;
    public Light roomLight;
    public BGCcMath curve;

    // Start off in the chair. For `cursor.DistanceRatio`:
    // - 0 is at the chair
    // - 1 is at the light switch
    private BGCcCursor cursor;
    public Animator npcAnimator;
    private SpriteRenderer npcSpriteRenderer;

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
        npcSpriteRenderer = npcAnimator.GetComponent<SpriteRenderer>();
        gameOverState.sprite = inGameSprite;
    }

    // Update is called once per frame
    void Update()
    {
        npcSpriteRenderer.flipX = !roomLight.enabled; // if moving back from light, or sitting, face left
        if (roomLight.enabled)
        {
            if (0 < leaveDeskDelayLeft)
            {
                leaveDeskDelayLeft -= Time.deltaTime;
            }
            else
            {
                npcAnimator.SetBool("walking", true);
                // move to turn off light
                cursor.Distance += walkingSpeed;
                tableBlockers.ForEach(blocker => blocker.SetActive(false));

                if (1 <= cursor.DistanceRatio)
                {
                    // at the light
                    roomLight.enabled = false;
                }
            }
        }
        else
        {
            // move back to chair
            cursor.Distance -= walkingSpeed;

            var atDesk = cursor.DistanceRatio <= 0;
            if (atDesk)
            {
                if (!complete) {
                    tableBlockers.ForEach(blocker => blocker.SetActive(true));
                }
                npcAnimator.SetBool("walking", false);

                leaveDeskDelayLeft = leaveDeskDelay;
            }
        }
    }

    public SpriteRenderer gameOverState;
    public Sprite inGameSprite, gameOverSprite;
    private void FixedUpdate()
    {
        if (keyboardTrigger.enabled && keyboardTrigger.complete)
        {
            keyboardTrigger.enabled = false;
            gameOverState.sprite = gameOverSprite;
            complete = true;
            tableBlockers.ForEach(blocker => blocker.SetActive(false));
            npcAnimator.SetBool("sad", true);
        }
    }
}