using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController globalController;
    public PlayerController player;
    public SpeechController2D speechController;
    public Canvas textCanvas;
    public GameObject textPrefab;
    public GameAudioController audioController;
    public GameObject questLogContainer;
    public AchievementLogController achievements;
    internal Animator cmAnimator;

    public float timeStart;
    void Awake()
    {
        globalController = this;
        cmAnimator = GetComponent<Animator>();
        audioController = GetComponent<GameAudioController>();
    }

    public void TriggerSpeech(SpeechEvent speechEvent)
    {
        speechController.Display(speechEvent);
    }

    public void StartGame(string cmTrigger)
    {
        player.controlEnabled = true;
        cmAnimator.SetTrigger(cmTrigger);
        questLogContainer.SetActive(true);
        timeStart = Time.time;
    }

    public void CloseAchievements()
    {
        achievements.achievementLogContainer.SetActive(false);
        player.controlEnabled = true;
    }
}
