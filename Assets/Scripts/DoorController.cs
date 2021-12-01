using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    internal Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        Debug.Log("opening " + name);
        animator.SetTrigger("Open");
    }
}
