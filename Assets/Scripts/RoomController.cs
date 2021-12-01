using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    enum Room
    {
        Bedroom,
        ComputerRoom,
    }

    internal Collider _collider;
    public List<GameObject> fadeOnEnter;
    public string cmTrigger;

    private Collider GenerateBoundingCollider()
    {
        // generate new collider that wraps everything
        var bounds = new Bounds();
        foreach (var o in GetComponentsInChildren<Collider>())
        {
            if (bounds.max == bounds.min) // bounds have not been initialised
            {
                bounds = o.bounds;
            }
            else
            {
                bounds.Encapsulate(o.bounds);
            }
        }
        var newCollider = gameObject.AddComponent<BoxCollider>();
        newCollider.center = transform.InverseTransformPoint(bounds.center);
        newCollider.size = bounds.size - new Vector3(0.3f, 0, 0.3f); // shrink it just a little because shared walls are annoying
        newCollider.isTrigger = true;
        return newCollider;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null)
        {
            _collider = GenerateBoundingCollider();
        }

        HandleRoomExit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleRoomExit()
    {
        foreach (var cs in fadeOnEnter)
        {
            foreach (var c in cs.GetComponentsInChildren<MeshRenderer>())
            {
                c.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
        foreach (var c in GetComponentsInChildren<Outline>())
        {
            c.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameController.globalController.player.gameObject && GameController.globalController.player.controlEnabled)
        {
            if (cmTrigger != null)
            {
                GameController.globalController.cmAnimator.SetTrigger(cmTrigger);
            }

            foreach (var cs in fadeOnEnter)
            {
                foreach (var c in cs.GetComponentsInChildren<MeshRenderer>())
                {
                    c.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
            foreach (var c in GetComponentsInChildren<Outline>())
            {
                c.enabled = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameController.globalController.player.gameObject)
        {
            HandleRoomExit();
        }
    }
}
