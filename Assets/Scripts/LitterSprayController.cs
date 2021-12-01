using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitterSprayController : MonoBehaviour
{   
    public GameObject player;
    public Vector3 offset;

    void Start()
    {
        if (!player) {
            player =  GameObject.Find("Player");
        }
        Destroy(gameObject, 60);
    }

    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
