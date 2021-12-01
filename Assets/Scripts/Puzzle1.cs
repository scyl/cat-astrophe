using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1 : MonoBehaviour
{
    public Collider floor;
    public List<Collider> objects;
    public bool complete;
    public DoorController doorToOpen;

    void FixedUpdate()
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            var o = objects[i];
            if (o.bounds.Intersects(floor.bounds))
            {
                objects.RemoveAt(i);
            }
        }
        if (!complete && objects.Count == 0)
        {
            complete = true;
            doorToOpen.OpenDoor();
        }
    }
}
