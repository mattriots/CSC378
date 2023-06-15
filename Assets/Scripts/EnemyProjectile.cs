using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private int damage;


    private float lifetime;
    private Animator animator;
    private BoxCollider2D collider;
    private bool hit;

    public PlayerCombat playerHealth;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        Debug.Log("in activate projectile");
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) 
        {
            return;
        }

        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime) 
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Debug.Log("Hit player with shuriken");
            hit = true;
            gameObject.SetActive(false);
            // playerHealth = collider.GetComponent<PlayerCombat>();
            playerHealth.TakeDamage(damage);
        }
    }

    private void Deactivate() 
    {
        gameObject.SetActive(false);
    }
}
