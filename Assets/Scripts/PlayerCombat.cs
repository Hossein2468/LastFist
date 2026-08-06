using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    private float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        lastAttackTime = -attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            Debug.Log("Cooldown!");
            return;
        }

        lastAttackTime = Time.time;

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

        Debug.Log("Attack time: " + Time.time);
        Debug.Log(hitEnemies.Length);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
