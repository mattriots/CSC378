using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 25;
    public float attackRate = 50f;
    float nextAttackTime = 0f;
    public int maxHealth = 100;
    public int currentHealth;
    // Update is called once per frame

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        currentHealth = maxHealth;
    }

    void Update()
{
    if (Time.time >= nextAttackTime) 
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            Attack();
            nextAttackTime = Time.time + 8f / attackRate;
            // Debug.Log("attackRate = " + attackRate);
            // Debug.Log("Time.time = " + Time.time);
            // Debug.Log("nextAttackTime = " + nextAttackTime);
        }
    }
}   

    void Attack() 
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        foreach(Collider2D enemy in hitEnemies) 
        {
            enemy.GetComponent<Henchman>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage) {
        currentHealth = currentHealth - damage;

        Debug.Log("Player took " + damage + " damage");
    }

    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null){
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
