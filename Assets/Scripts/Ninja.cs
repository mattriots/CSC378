using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform throwpoint;
    [SerializeField] private GameObject[] shurikens;

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

    private void Update() 
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {   
                cooldownTimer = 0;
                RangedAttack();
            }
        }

    }

    public void TakeDamage(int damage) {
        currentHealth = currentHealth - damage;

        Debug.Log("Enemy took " + damage + " damage");
        Debug.Log("Enemy health is now " + currentHealth);
        // animator.SetTrigger("Hurt");
        // PlayRandomHitSound();

        if (currentHealth <= 0) {
            Die();
        }
    }

        void Die() 
    {
        Debug.Log("Enemy died");
        // animator.SetTrigger("Death");
        // PlayRandomDeathSound();

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void RangedAttack() 
    {
        Debug.Log("Throw shuriken");
        cooldownTimer = 0;
        shurikens[FindShuriken()].transform.position = throwpoint.position;
        Debug.Log("throwpoint");

        shurikens[FindShuriken()].GetComponent<EnemyProjectile>().ActivateProjectile();
        Debug.Log("activate projectile");

    }

    private int FindShuriken()
    {
        for (int i = 0; i < shurikens.Length; i++) 
        {
            if (!shurikens[i].activeInHierarchy) 
            {
                return i;
            }
        }
        return 0;
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

}
