using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;

    public float attackInterval = 3.0f;
    public float lastAttackTime = 0.0f;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 25;

    public float speed = 2f; // The speed at which the NPC moves
    public Transform leftPoint; // The left-most point the NPC will move to
    public Transform rightPoint; // The right-most point the NPC will move to
    private bool movingRight = true; // Whether the NPC is currently moving right or left

    void Awake() {
        animator = GetComponent<Animator>();
        Debug.Log("Enemy health is " + currentHealth);
    }

    void Start() 
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth = currentHealth - damage;

        Debug.Log("Enemy took " + damage + " damage");
        Debug.Log("Enemy health is now " + currentHealth);
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() 
    {
        Debug.Log("Enemy died");
        animator.SetTrigger("Death");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        // if (Time.time >= lastAttackTime + attackInterval  ) {
        //     Attack();
        //     lastAttackTime = Time.time;
        // }
        //animator.SetTrigger("Walk");
    }

    // void Walk(){
    //     if (movingRight)
    //     {
    //         transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, speed * Time.deltaTime);
    //     }
    //     else
    //     {
    //         transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, speed * Time.deltaTime);
    //     }

    //     // Check if the NPC has reached its target point, and reverse its direction if it has
    //     if (transform.position == rightPoint.position)
    //     {
    //         movingRight = false;
    //         GetComponent<Animator>().SetTrigger("Walk"); // Trigger the MoveLeft animation clip
    //     }
    //     else if (transform.position == leftPoint.position)
    //     {
    //         movingRight = true;
    //         GetComponent<Animator>().SetTrigger("Walk"); // Trigger the MoveRight animation clip
    //     }
    
    // }

    void Attack() 
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        foreach(Collider2D player in hitPlayer) 
        {
            player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null){
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
