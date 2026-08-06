using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
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
            Die();
        }
    }
    void Die()
    {
        Debug.Log(gameObject.name + " Died");
    }
}
