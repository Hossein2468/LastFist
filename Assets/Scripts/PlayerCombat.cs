using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private enum AttackType
    {
        None,
        Fist,
        Kick
    }

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Combo")]
    [SerializeField] private float comboResetTime = 1f;
    private int comboStep = 0;

    private AttackType firstAttack = AttackType.None;
    private AttackType secondAttack = AttackType.None;

    private List<AttackType> comboInputs = new List<AttackType>();
    private AttackType bufferedAttack = AttackType.None;

    private bool comboWindowOpen;

    private float lastAttackTime;
    public bool isAttacking { get; private set; }

    [Header("Damage")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int attackDamage = 10;
    // KnockBack will be added in future.
    //[SerializeField] private float knockBackForce = 3f;

    private readonly List<Health> hitTargets = new List<Health>();

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckComboReset();

        if (Input.GetKeyDown(KeyCode.J))
        {
            HandleAttackInput(AttackType.Fist);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HandleAttackInput(AttackType.Kick);
        }
    }

    void HandleAttackInput(AttackType attackType)
    {
        if (!isAttacking)
        {
            StartAttack(attackType);
            return;
        }

        if (comboWindowOpen)
        {
            if (IsValidNextAttack(attackType))
            {
                bufferedAttack = attackType;
            }
        }
    }

    void StartAttack(AttackType attackType)
    {
        comboInputs.Add(attackType);
        comboStep++;

        hitTargets.Clear();

        if (comboStep == 1)
        {
            firstAttack = attackType;
            secondAttack = AttackType.None;
        }
        else if (comboStep == 2)
        {
            secondAttack = attackType;
        }

        isAttacking = true;

        lastAttackTime = Time.time;

        comboWindowOpen = false;

        bufferedAttack = AttackType.None;

        animator.SetInteger("ComboStep", comboStep);

        if (attackType == AttackType.Fist)
        {
            animator.SetTrigger("FistAttack");
        }
        else if (attackType == AttackType.Kick)
        {
            animator.SetTrigger("KickAttack");
        }

    }

    bool IsValidNextAttack(AttackType nextAttack)
    {
        if (comboInputs.Count == 0)
        {
            return true;
        }
        if (comboInputs.Count == 1)
        {
            return true;
        }
        if (comboInputs.Count == 2)
        {
            AttackType first = comboInputs[0];
            AttackType second = comboInputs[1];

            if (first == AttackType.Fist && second == AttackType.Fist)
            { return nextAttack == AttackType.Kick; }
            if (first == AttackType.Fist && second == AttackType.Kick)
            { return nextAttack == AttackType.Fist; }
            if (first == AttackType.Kick && second == AttackType.Fist)
            { return nextAttack == AttackType.Fist; }
            if (first == AttackType.Kick && second == AttackType.Kick)
            { return nextAttack == AttackType.Fist; }
        }
        return false;
    }
    
    public void OpenComboWindow()
    {
        comboWindowOpen = true;
    }

    public void CloseComboWindow()
    {
        comboWindowOpen = false;
    }

    public void EndAttack()
    {
        comboWindowOpen = false;
        
        if (bufferedAttack != AttackType.None && comboStep < 3)
        {
            AttackType nextAttack = bufferedAttack;

            bufferedAttack = AttackType.None;

            StartAttack(nextAttack);

            return;
        }

        FinishCombo();
    }

    void FinishCombo()
    {
        comboInputs.Clear();

        isAttacking = false;

        comboWindowOpen = false;

        bufferedAttack = AttackType.None;

        comboStep = 0;

        firstAttack = AttackType.None;
        secondAttack = AttackType.None;
    }

    void CheckComboReset()
    {
        if (comboStep == 0)
        {
            return;
        }

        if (Time.time - lastAttackTime > comboResetTime)
        {
            FinishCombo();
        }
    }

    public void DealDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // Maybe GetComponentInParent<> in future.
            Health health = enemy.GetComponent<Health>();
            if (health == null)
            {
                continue;
            }
            if (hitTargets.Contains(health))
            {
                continue;
            }

            health.TakeDamage(attackDamage);

            hitTargets.Add(health);

            Debug.Log("Player Hit: " + health.gameObject.name);
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
