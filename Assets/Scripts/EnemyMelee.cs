using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
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

    [SerializeField] private Transform enemy;

    //References
    private Animator animator;
    //private Health playerHealth;
    //private EnemyPatrol enemyPatrol;

    public int maxHealth = 100;
    public int currentHealth;
    public GameObject player;


    public float speed = 2f; // The speed at which the NPC moves
    public Transform leftPoint; // The left-most point the NPC will move to
    public Transform rightPoint; // The right-most point the NPC will move to
    private bool movingLeft = true; // Whether the NPC is currently moving right or left
    private bool isActive = false;
   
    private SpriteRenderer spriteRenderer;
    public PlayerCombat playerHealth;

    public AudioClip[] hitSounds;
    public AudioClip[] deathSounds; 
    private AudioSource audioSource;

    private Vector3 initScale;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //enemyPatrol = GetComponentInParent<EnemyPatrol>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        initScale = enemy.localScale;
  
    }

    void PlayRandomHitSound()
    {
        int index = Random.Range(0, hitSounds.Length); 
        AudioClip clip = hitSounds[index]; 
        audioSource.PlayOneShot(clip); 
    }

    void PlayRandomDeathSound()
    {
        int index = Random.Range(0, deathSounds.Length);
        AudioClip clip = deathSounds[index]; 
        audioSource.PlayOneShot(clip);
    }

    public void TakeDamage(int damage) {
        currentHealth = currentHealth - damage;

        Debug.Log("Enemy took " + damage + " damage");
        Debug.Log("Enemy health is now " + currentHealth);
        animator.SetTrigger("Hurt");
        PlayRandomHitSound();

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() 
    {
        Debug.Log("Enemy died");
        animator.SetTrigger("Death");
        PlayRandomDeathSound();

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }


    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            Debug.Log("Player is in sight");
            if (cooldownTimer >= attackCooldown)
            {
                // if(!movingLeft)
                //     spriteRenderer.flipX = true;
                cooldownTimer = 3f;
                animator.SetTrigger("Attack");
            }
        }

        else{ 
            //animator.ResetTrigger("Attack");
            Walk();
            //animator.SetBool("Attack", false);

        }

    }


     void Walk(){

        int _direction;

        if (movingLeft)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, speed * Time.deltaTime);
            animator.SetTrigger("Walk");
                    // Check if the NPC has reached its target point, and reverse its direction if it has
                if (transform.position == leftPoint.position)
                {
                    _direction = -1;
                    movingLeft = false;
                    //spriteRenderer.flipX = true;
                    enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
                        initScale.y, initScale.z);
                    //animator.SetTrigger("Walk"); // Trigger the MoveLeft animation clip
                }
            
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, speed * Time.deltaTime);
            animator.SetTrigger("Walk");

            if (transform.position == rightPoint.position)
                {
                    _direction = 1;
                    movingLeft = true;
                    //spriteRenderer.flipX = true;
                    enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
                        initScale.y, initScale.z);
                    //animator.SetTrigger("Walk"); // Trigger the MoveRight animation clip
                }
        }
    
    }


    private bool PlayerInSight()
    {

        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        

        if (hit.collider != null){
            playerHealth = hit.transform.GetComponent<PlayerCombat>();
        
        }

        return hit.collider != null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


    private void DamagePlayer()
    {
        if (PlayerInSight()){
            //Debug.Log("Player is damage");
            playerHealth.TakeDamage(damage);
        }
    }
    
}