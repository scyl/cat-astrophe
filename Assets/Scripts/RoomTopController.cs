using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTopController : MonoBehaviour
{
    public string hitString;
    internal Collider _collider;
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameController.globalController.player.gameObject)
        {
            GameController.globalController.achievements.roomTops.Add(hitString);
        }
    }
}
