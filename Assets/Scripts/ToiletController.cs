using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletController : MonoBehaviour
{
    public bool flushed;

    void Interact()
    {
        flushed = true;
        GameController.globalController.audioController.Flush();
    }
}
