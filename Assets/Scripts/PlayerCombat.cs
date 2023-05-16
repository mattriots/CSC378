using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    private Transform spawnPoint;
    private Transform playerPos;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 25;
    public float attackRate = 50f;
    float nextAttackTime = 0f;
    public int maxHealth = 125;
    public int currentHealth;
    public Playerhealth healthbar;
    public bool isAlive = true;

    public AudioClip[] attackSounds;
    private AudioSource audioSource;
    public AudioClip[] hurtSounds;

    // Update is called once per frame

    void Awake() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
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

    void PlayRandomAttackSound()
    {
        int index = Random.Range(0, attackSounds.Length); 
        AudioClip clip = attackSounds[index]; 
        audioSource.PlayOneShot(clip); 
    }

    void PlayRandomHurtSound()
    {
        int index = Random.Range(0, hurtSounds.Length); // Choose a random index
        AudioClip clip = hurtSounds[index]; // Get the audio clip at the random index
        audioSource.PlayOneShot(clip); // Play the audio clip
    }

    void Attack() 
    {
        animator.SetTrigger("Attack");
        PlayRandomAttackSound();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        foreach(Collider2D enemy in hitEnemies) 
        {
            enemy.GetComponent<EnemyMelee>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage) {
        animator.SetTrigger("Hurt");
        PlayRandomHurtSound();

        currentHealth = currentHealth - damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("Player took " + damage + " damage");
    
        if(currentHealth < 0) {
            animator.SetTrigger("Dead");
            healthbar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            playerPos.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
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
