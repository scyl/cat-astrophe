using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController2D : MonoBehaviour
{
    public Sprite thought, speech;
    public Image speechBubble;
    public GameObject text;

    public enum Mode {Speech, Thought};

    private Text textComponent;

    private List<SpeechEvent> speechEvents = new List<SpeechEvent>();

    void Awake()
    {
        textComponent = text.GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        while (speechEvents.Count != 0 && Time.time > speechEvents[0].displayTime)
        {
            speechEvents.RemoveAt(0);
        }

        if (speechEvents.Count == 0)
        {
            speechBubble.gameObject.SetActive(false);
            text.SetActive(false);
        }
        else
        {
            speechBubble.gameObject.SetActive(true);
            text.SetActive(true);

            var e = speechEvents[0];
            textComponent.text = e.text;
            if (e.mode == Mode.Speech)
            {
                speechBubble.sprite = speech;
            }
            else
            {
                speechBubble.sprite = thought;
            }
        }
    }

    public void Display(SpeechEvent input)
    {
        // very cheekily reinterpret displayTime as when to stop displaying
        speechEvents.Add(new SpeechEvent(input.text, input.displayTime + Time.time, input.mode));
    }
}
