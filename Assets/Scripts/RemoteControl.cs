using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControl : MonoBehaviour
{
    public List<GameObject> screens;
    public bool initialOn = true;
    private bool lastState = true;
    // Start is called before the first frame update
    void Start()
    {
        lastState = initialOn;
        screens.ForEach(delegate(GameObject screen)
        {
            screen.SetActive(initialOn);
        });
    }

    public void Interact()
    {
        screens.ForEach(delegate(GameObject screen)
        {
            screen.SetActive(true);
        });
    }
}
