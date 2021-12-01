using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public List<Light> lights;
    public bool initialOn = true;
    private bool lastState = true;
    // Start is called before the first frame update
    void Start()
    {
        lastState = initialOn;
        lights.ForEach(delegate(Light light)
        {
            light.enabled = initialOn;
        });
    }


    public void Interact()
    {
        lights.ForEach(delegate(Light light)
        {
            light.enabled = !light.enabled;
        });
    }
}
