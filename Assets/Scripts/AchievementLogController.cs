using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementLogController : MonoBehaviour
{
    public Text achievementLog;
    public GameObject achievementLogContainer, questLogContainer;
    internal QuestLogController questLog;
    private ToiletController toilet;
    void Start()
    {
        questLog = GetComponent<QuestLogController>();
        toilet = FindObjectOfType<ToiletController>();
    }

    private string GetBody(string body, string fallback, bool c)
    {
        if (c) return body;
        else return fallback;
    }

    private string MakeLine(string title, string body, bool c)
    {
        return title + " - " + GetBody(body, "???", c) + "\n";
    }
    private string MakeLineHint(string title, string body, string hint, bool c)
    {
        return title + " - " + GetBody(body, hint, c) + "\n";
    }

    private bool doneskies = false;
    private float timeToComplete;

    private int CleanHits()
    {
        // TODO shower on, sink on
        return toilet.flushed ? 1 : 0 + 0 + 0;
    }

    public HashSet<string> roomTops = new HashSet<string>();
    public HashSet<Rigidbody> bodiesMoved = new HashSet<Rigidbody>();

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!doneskies && questLog.complete)
        {
            doneskies = true;
            timeToComplete = Time.time - GameController.globalController.timeStart;
            questLogContainer.SetActive(false);
            achievementLogContainer.SetActive(true);
            GameController.globalController.player.controlEnabled = false;
        }

        var text = "";
        text += "Time to complete: " + timeToComplete + " seconds\n";

        text += "\nCommon:\n";
        text += MakeLine("Let meow-t!", "escape the bedroom", questLog.puzzle1.complete);
        text += MakeLine("You can’t paws the game...", "make the human game over in their video game", questLog.computerRoomPuzzle.complete);
        text += MakeLine("Litter-ally the worst", "Activate the roomba by spilling kitty litter everywhere.", questLog.roombaController.complete);
        text += MakeLine("Just feline hungry", "Steal the food off the dining table.", questLog.foodPuzzle.complete);
        // text += MakeLine("What a cat-ch!", "successfully catch all bugs in the garage", false);

        text += "\nRare:\n";
        text += MakeLineHint("Sanitation ex-purr-t", "flush toilet, turn on the shower and sink.", CleanHits() + " / 3", CleanHits() >= 3);
        text += MakeLineHint("Catatonic", "stay on the roomba for 1 minute.", ((int)questLog.roombaController.totalRideTime) + " / 60", questLog.roombaController.totalRideTime >= 60);
        // text += MakeLine("Chonker", "eat all food items in the house.", false);

        text += "\nEpic:\n";
        text += MakeLineHint("Cathletic", "jump onto all of the highest points reachable in each room", roomTops.Count + " / 3", roomTops.Count >= 3);
        // text += MakeLine("Grumpy cat", "???", false);

        text += "\nLegendary:\n";
        text += MakeLineHint("De-meow-litions specialist", "Use the ‘swipe’ ability 100 times", GameController.globalController.player.swipeCount + " / 100", GameController.globalController.player.swipeCount >= 100);
        text += MakeLineHint("Polite cat", "Complete all objectives without knocking any unnecessary items over", bodiesMoved.Count + " / 4", bodiesMoved.Count <= 4); // number of items needed for bedroom puzzle

        achievementLog.text = text;
    }
}
