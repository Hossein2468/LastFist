using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float bodyRadius;
    public Transform cameraTransform;
    public LayerMask enemyLayer;

    private PlayerCombat playerCombat;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCombat = GetComponent<PlayerCombat>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        movement = (forward * v + right * h).normalized;

        if (playerCombat.isAttacking)
        {
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            animator.SetFloat("Speed", movement.magnitude);
        }
    }

    private void FixedUpdate()
    {
        if (!playerCombat.isAttacking)
        {
            Vector3 nextPosition = rb.position + movement * speed * Time.fixedDeltaTime;

            rb.MovePosition(nextPosition);
        }

        if (movement != Vector3.zero)
        {
            Quaternion targerRotation = Quaternion.LookRotation(movement);

            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targerRotation, rotateSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(smoothRotation);
        }
    }

    public Vector3 GetMovementDirection()
    {
        return movement;
    }
}