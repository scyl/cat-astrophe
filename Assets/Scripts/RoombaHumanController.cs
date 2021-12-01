using BansheeGz.BGSpline.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaHumanController : MonoBehaviour
{
    public GameObject litter;
    public RoombaController roomba;

    void Update()
    {
    }
    public void Interact()
    {
        GameController.globalController.audioController.Meow();
        if (litter.activeInHierarchy && !roomba.isRunning)
        {
            roomba.StartRunning();
            GameController.globalController.TriggerSpeech(
                new SpeechEvent("Roomba go", 5f, SpeechController2D.Mode.Speech)
            );
        }
    }
}
