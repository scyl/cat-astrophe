using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool controlEnabled = true;
    internal CharacterController controller;
    private Vector3 playerVelocity;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float weight = 2.0f;
    public float interactRange = 0.5f;



    private Camera mainCamera;

    private float distToGround;
    internal Animator animator;

    internal SpriteRenderer sprite;
    public float swipeRadius = 1.0f;
    public float swipeCooldown = 1.0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private Vector3 getMoveAroundCamera(Vector3 movement)
    {
        // we want to move in the space of the camera
        var toPlayer = transform.position - Camera.main.transform.position;
        toPlayer.y = 0;
        toPlayer.Normalize();
        return toPlayer * movement.z + Quaternion.Euler(0, 90, 0) * toPlayer * movement.x;
    }

    void FixedUpdate()
    {
        // https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
        var groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        animator.SetBool("jumping", !groundedPlayer);

        Vector3 localMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (!controlEnabled)
        {
            localMove = Vector3.zero;
        }
        if (Mathf.Abs(localMove.x) > 0.01)
        {
            if (localMove.x < 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }

        animator.SetFloat("velocityX", Mathf.Abs(localMove.x));
        animator.SetFloat("velocityZ", Mathf.Abs(localMove.z));
        animator.SetFloat("totalVelocity", Mathf.Abs(localMove.magnitude));
        animator.SetBool("walkingBack", localMove.z > 0 && Mathf.Abs(localMove.x) < localMove.z);

        Vector3 move = getMoveAroundCamera(localMove);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (controlEnabled && Input.GetButton("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (controlEnabled && Input.GetKey(KeyCode.LeftControl))
        {
            Swipe();
        }
    }

    void Update() {
        if (!controlEnabled)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Interact();
        }
    }

    private float noSwipeTil = 0f;
    public int swipeCount;
    private void Swipe()
    {
        if (Time.time < noSwipeTil) return;
        noSwipeTil = Time.time + swipeCooldown;
        GameController.globalController.audioController.Swipe();
        animator.SetTrigger("swipe");
        swipeCount++;
        foreach (var c in Physics.OverlapSphere(transform.position, swipeRadius))
        {
            if (c.gameObject != gameObject)
            {
                var other = c.gameObject.GetComponent<Rigidbody>();
                if (other == null) {
                    other = c.gameObject.GetComponentInParent<Rigidbody>();
                    if (other == null) continue;
                };
                var force = new Vector3(Random.value - 0.5f, Random.value / 2, Random.value - 0.5f).normalized * pushPower * 10;
                other.AddForce(force);
                GameController.globalController.achievements.bodiesMoved.Add(other);
            }
        }
    }

    // this script pushes all rigidbodies that the character touches
    public float pushPower = 2.0f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html
        Rigidbody body = hit.collider.attachedRigidbody;

        // TODO send this less?
        hit.gameObject.SendMessage("PlayerHit", hit, SendMessageOptions.DontRequireReceiver);

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        //if (hit.moveDirection.y < -0.3)
        //{
        //    return;
        //}

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        // Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        // body.velocity = pushDir * pushPower;

        Vector3 force;
        // We use gravity and weight to push things down, we use
        // our velocity and push power to push things other directions
        if (hit.moveDirection.y < -0.3 
            && (hit.controller.collisionFlags != CollisionFlags.Sides && hit.controller.collisionFlags != CollisionFlags.None)) {
            force = new Vector3(0, 0.5f, 0) * gravityValue * weight;
        } else {
            // force = hit.controller.velocity.magnitude * hit.normal * -1 * pushPower;
            // force = hit.controller.velocity * pushPower;
            force = hit.moveDirection.magnitude * hit.normal * -1 * pushPower;
        }

        // Apply the push
        body.AddForceAtPosition(force, hit.point);
        GameController.globalController.achievements.bodiesMoved.Add(body);
    }

    private void Interact()
    {
        List<Collider> inRange = new List<Collider>(Physics.OverlapSphere(transform.position, interactRange));
        inRange.ForEach(delegate(Collider collider)
        {
            if (collider.gameObject.tag == "Interactable")
            {
                collider.gameObject.SendMessage("Interact");
            }
        });
    }
}
