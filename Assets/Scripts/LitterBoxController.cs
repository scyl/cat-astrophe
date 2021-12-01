using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitterBoxController : MonoBehaviour
{
    public GameObject litter;
    public GameObject litterSprayer;
    void Start()
    {
        litter.SetActive(false);
    }

    public void Interact()
    {
        litter.SetActive(true);
        Instantiate(litterSprayer);
    }
}
