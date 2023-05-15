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

    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    public Transform attackPoint;
    public Transform facePlayer;
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

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

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
        //calculate distance to player 
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log(distanceToPlayer);
        if(distanceToPlayer < triggerDist ){
            if(movingLeft == false){
                spriteRenderer.flipX = true;
            }
            Attack(); 
            //Debug.Log("Close to the player");
        }
    
       else{ 
        animator.ResetTrigger("Attack");
        Walk();
        //animator.SetBool("Attack", false);
        if(movingLeft == false){
            spriteRenderer.flipX = false;
        }
        }

        
    }

    void Walk(){

        if (movingLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, speed * Time.deltaTime);
            animator.SetTrigger("Walk");
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
            animator.SetTrigger("Walk");

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
        //Debug.Log("Attacked is called");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        foreach(Collider2D player in hitPlayer) 
        {
            Debug.Log("Hitting Player");
            player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }

    
    }

     private bool hitPlayer()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            Debug.Log("Test");
            //playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (hitPlayer())
            Debug.Log("Player is hit");
    }

    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null){
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
