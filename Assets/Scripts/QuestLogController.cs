using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogController : MonoBehaviour
{
    public Text questLog;

    public string bedRoomPuzzleText = "Trash the bedroom";
    public string roombaPuzzleText = "Ride the vacuum robot";
    public string foodPuzzleText = "Jump on the dining table";
    public string computerRoomPuzzleText = "Force a game over";

    public string completeText = "☑";
    public string incompleteText = "☒";

    internal Puzzle1 puzzle1;
    internal RoombaController roombaController;
    internal FoodPuzzle foodPuzzle;
    internal ComputerRoomPuzzle computerRoomPuzzle;
    // Start is called before the first frame update
    void Start()
    {
        puzzle1 = FindObjectOfType<Puzzle1>();
        roombaController = FindObjectOfType<RoombaController>();
        foodPuzzle = FindObjectOfType<FoodPuzzle>();
        computerRoomPuzzle = FindObjectOfType<ComputerRoomPuzzle>();
    }
    
    private string GetCompletion(bool c)
    {
        if (c) return completeText;
        else return incompleteText;
    }

    private string MakeLine(string text, bool c)
    {
        return GetCompletion(c) + " " + text + "\n";
    }

    // public so we can test
    public bool complete = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (puzzle1.complete && computerRoomPuzzle.complete && roombaController.complete && foodPuzzle.complete)
        {
            complete = true;
        }
        var text = "";
        text += MakeLine(bedRoomPuzzleText, puzzle1.complete);
        text += MakeLine(computerRoomPuzzleText, computerRoomPuzzle.complete);
        text += MakeLine(roombaPuzzleText, roombaController.complete);
        text += MakeLine(foodPuzzleText, foodPuzzle.complete);
        questLog.text = text;
    }
}
