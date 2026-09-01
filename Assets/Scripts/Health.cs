using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private Animator animator;
    private int health;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log($"{gameObject.name} HP = {health}");

        if (health <= 0)
        {
            // This problem will be fixed after implementng enemy animator
            if (animator != null)
            {
                Die();
            }
        }
        else
        {
            // This problem will be fixed after implementng enemy animator
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }
    }
    void Die()
    {
        animator.SetTrigger("Death");

        Debug.Log(gameObject.name + " Died");
    }
}
