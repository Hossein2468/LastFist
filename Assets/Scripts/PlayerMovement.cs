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

    private Rigidbody rb;
    private Animator anim;
    private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
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

        anim.SetFloat("Speed", movement.magnitude);
    }

    private void FixedUpdate()
    {
        Vector3 nextPosition = rb.position + movement * speed * Time.fixedDeltaTime;
        bool hit = Physics.CheckSphere(nextPosition, bodyRadius, enemyLayer);

        if (!hit)
        {
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