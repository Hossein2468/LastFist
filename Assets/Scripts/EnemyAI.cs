using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Hit,
    Dead
}

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed;
    public float rotateSpeed; 
    public float stopDistance = 2f;

    private EnemyCombat combat;
    private EnemyState currentState;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        combat = GetComponent<EnemyCombat>();

        ChangeState(EnemyState.Chase);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Hit:
                Hit();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;

        Debug.Log("Enemy State: " + currentState);
    }
    private void Idle()
    {

    }

    private void Chase()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stopDistance)
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(smoothRotation);
    }

    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion smoothRotation =  Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(smoothRotation);
        }

        combat.Attack();
    }

    private void Hit()
    {

    }

    private void Dead()
    {

    }
}
