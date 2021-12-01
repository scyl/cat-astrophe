using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBlockerController : MonoBehaviour
{
    public GameObject speechController;
    public SpeechEvent speechEvent;
    private void PlayerHit(ControllerColliderHit hit)
    {
        if (speechEvent != null)
        {
            speechController.SendMessage("Display", speechEvent);
        }
    }
}
