using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 4f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Vector3 dashDirection;
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private Animator animator;
    private bool isDashing;
    private float lastDashTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartDash();
        }
    }

    public void StartDash()
    {
        if (isDashing)
        {
            return;
        }
        
        if (Time.time - lastDashTime < dashCooldown)
        {
            return;
        }

        Vector3 movementDirection = playerMovement.GetMovementDirection();

        if (movementDirection != Vector3.zero)
        {
            dashDirection = movementDirection;
        }
        else
        {
            dashDirection = transform.forward;
        }

        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        animator.SetBool("IsDashing", true);

        float dashSpeed = dashDistance / dashDuration;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isDashing = false;
        animator.SetBool("IsDashing", false);
    }
}
