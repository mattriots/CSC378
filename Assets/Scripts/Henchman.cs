using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Henchman : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    public GameObject player;

    public float attackInterval = 2.0f;
    public float lastAttackTime = 0.0f;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 25;

    public float speed = 2f; // The speed at which the NPC moves
    public Transform leftPoint; // The left-most point the NPC will move to
    public Transform rightPoint; // The right-most point the NPC will move to
    private bool movingLeft = true; // Whether the NPC is currently moving right or left
    private bool isActive = false;
    public float triggerDist = 0.01f;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        animator = GetComponent<Animator>();
        Debug.Log("Enemy health is " + currentHealth);
    }

    void Start() 
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        ///calculate distance to player 
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if(distanceToPlayer <= triggerDist){
            Debug.Log("Close to the player");
            Attack();
        }
        // {
        //     //animator.SetBool("Attack", true);
        //     Attack();
        //     //animator.SetBool("Walk", false);
        //     Debug.Log("close to player");
        //     isActive = true;
        // }
        // else if (distanceToPlayer > triggerDist || isActive){
        //     Walk();
        //     //animator.SetBool("Walk", true);
        //     isActive = false;
        // }

        //Put attack inside walk??
    
       
        Walk();
        
    
        
    }

    void Walk(){
        
        animator.SetTrigger("Walk");
        if (movingLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, speed * Time.deltaTime);
                    // Check if the NPC has reached its target point, and reverse its direction if it has
                if (transform.position == leftPoint.position)
                {
                    movingLeft = false;
                    spriteRenderer.flipX = false;
                
                    //animator.SetTrigger("Walk"); // Trigger the MoveLeft animation clip
                }
            
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, speed * Time.deltaTime);

            if (transform.position == rightPoint.position)
                {
                    movingLeft = true;
                    spriteRenderer.flipX = true;
                    //animator.SetTrigger("Walk"); // Trigger the MoveRight animation clip
                }
        }    
      
    
    }

    void Attack() 
    {

        animator.SetTrigger("Attack");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        foreach(Collider2D player in hitPlayer) 
        {
            Debug.Log("Hitting Player");
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
