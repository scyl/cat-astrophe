using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public bool complete;
    private void PlayerHit(ControllerColliderHit hit)
    {
        if (!complete)
        {
            complete = true;
        }
    }
}
