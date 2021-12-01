using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    public bool lockY = true;

    private Camera main;

    private void Start()
    {
        main = Camera.main;
    }

    void LateUpdate()
    {
        var sameLevelPosition = main.transform.position;
        if (lockY)
        {
            sameLevelPosition.y = transform.position.y;
        }
        transform.LookAt(2 * transform.position - sameLevelPosition, Vector3.up);
        //Vector3 eulerAngles = transform.eulerAngles;
        //eulerAngles.z = 0;
        //transform.eulerAngles = eulerAngles;
    }
}
