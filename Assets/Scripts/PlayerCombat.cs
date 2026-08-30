using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 10;
    //public float attackCooldown = 0.5f;
    public float comboResetTime = 1f;
    public bool isAttacking { get; private set; }
    public LayerMask enemyLayer;
    public Transform attackPoint;

    private Animator animator;
    private int comboStep = 0;
    private float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            FistAttack();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            KickAttack();
        }
    }

    void FistAttack()
    {
        StartCombo();

        isAttacking = true;

        animator.SetInteger("ComboStep", comboStep);
        animator.SetTrigger("FistAttack");
    }

    void KickAttack()
    {
        StartCombo();

        isAttacking = true;

        animator.SetInteger("ComboStep", comboStep);
        animator.SetTrigger("KickAttack");
    }

    void StartCombo()
    {
        lastAttackTime = Time.time;

        comboStep++;

        if (comboStep > 3)
        {
            comboStep = 1;
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void DealDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
            Debug.Log(enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
