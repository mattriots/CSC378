using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Henchman : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    public GameObject player;

    public Transform facePlayer;

    public float speed = 2f; // The speed at which the NPC moves
    public Transform leftPoint; // The left-most point the NPC will move to
    public Transform rightPoint; // The right-most point the NPC will move to
    private bool movingLeft = true; // Whether the NPC is currently moving right or left
    private bool isActive = false;
   
    private SpriteRenderer spriteRenderer;
    public PlayerCombat playerHealth;

    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

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
<<<<<<< HEAD
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
        
=======

         //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                if(movingLeft == false)
                    spriteRenderer.flipX = true;
        
                Debug.Log("Close to the player");
                cooldownTimer = 0;
                animator.SetTrigger("Attack");
            }
        }
        //calculate distance to player 
        // float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // //Debug.Log(distanceToPlayer);
        // if(distanceToPlayer <= 1.5 ){

        //     if(movingLeft == false){
        //         spriteRenderer.flipX = true;
        //     }
        //     Attack(); 
        //     Debug.Log("Close to the player");
        // }
>>>>>>> jun-new
    
       else{ 
            //animator.ResetTrigger("Attack");
            Walk();
            //animator.SetBool("Attack", false);
            if(movingLeft == false){
                spriteRenderer.flipX = false;
            }
        }

        
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
<<<<<<< HEAD
        }    
      
=======
        }
>>>>>>> jun-new
    
    }

    // void Attack() 
    // {
    
    //     animator.SetTrigger("Attack");
    //     Debug.Log("Attacked is called");

    //     // Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
    //     // foreach(Collider2D player in hitPlayer) 
    //     // {
    //     //     Debug.Log("Hitting Player");
    //     //     Playerhealth.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
    //     // }
    //     //hitPlayer();

    
    // }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<PlayerCombat>();

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight()){
            //Debug.Log("Player is damage");
            playerHealth.TakeDamage(damage);
        }
    }

    // void OnDrawGizmosSelected() 
    // {
    //     if (attackPoint == null){
    //         return;
    //     }
    
    //     Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
