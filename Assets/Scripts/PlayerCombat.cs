using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Ranged Attack")]
    [SerializeField] private Transform throwpoint;
    [SerializeField] private GameObject[] shurikens;

    private float cooldownTimer = Mathf.Infinity;

    public int shurikenCount;

    // Update is called once per frame

    void Awake() 
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() 
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        shurikenCount = 0;

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

        if (Input.GetKeyDown(KeyCode.X) && shurikenCount > 0)
        {
            RangedAttack();
            nextAttackTime = Time.time + 4f / attackRate;
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

            if (enemy.GetComponent<EnemyMelee>()) 
            {
                enemy.GetComponent<EnemyMelee>().TakeDamage(attackDamage);
            } else if (enemy.GetComponent<Ninja>())
            {
                enemy.GetComponent<Ninja>().TakeDamage(attackDamage);
            }

        }
    }

    private void RangedAttack() 
    {
        shurikenCount--;
        cooldownTimer = 0;
        shurikens[FindShuriken()].transform.position = throwpoint.position;
        float direction = transform.localScale.x;
        shurikens[FindShuriken()].GetComponent<PlayerProjectile>().ActivateProjectile(direction);

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

    public void TakeDamage(int damage) {
        animator.SetTrigger("Hurt");
        PlayRandomHurtSound();

        currentHealth = currentHealth - damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("Player took " + damage + " damage");
    
        if(currentHealth < 0) {
            Die();
        }
    }

    public void Die() 
    {
        animator.SetTrigger("Dead");
        healthbar.SetHealth(maxHealth);
        currentHealth = maxHealth;
        // playerPos.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collided with the spike GameObject
        if (collision.CompareTag("Spike"))
        {
            // Handle spike collision logic here
            Die();
        }
    }

    public void healHealth(int healAmount)
    {
        currentHealth = currentHealth + healAmount;
        Debug.Log(currentHealth);
        if (currentHealth >= maxHealth) 
        {
            healthbar.SetHealth(maxHealth);
            currentHealth = maxHealth;
            Debug.Log(currentHealth);

        } else {
            healthbar.SetHealth(currentHealth);
            Debug.Log(currentHealth);

        }

    }

    public void addShuriken() 
    {
        shurikenCount += 5;
        Debug.Log("shuriken count: " + shurikenCount);
    }

    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null){
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
