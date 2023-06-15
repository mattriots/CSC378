using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private int damage;


    private float lifetime;
    private Animator animator;
    private BoxCollider2D collider;
    private bool hit;

    private PlayerCombat player;
    public float direction;
    [SerializeField] private LayerMask enemyLayers;


    private void Awake() 
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        player = GetComponent<PlayerCombat>();
    }

    public void ActivateProjectile(float playerDirection)
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        collider.enabled = true;
        if (playerDirection < 0) {
            direction = 1;
        } else {
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) 
        {
            return;
        }

        float movementSpeed = speed * Time.deltaTime;

        transform.Translate(direction * movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime) 
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemies") 
        {
            Debug.Log("Hit enemy with shuriken");
            hit = true;
            gameObject.SetActive(false);
            // playerHealth = collider.GetComponent<PlayerCombat>();
            collision.gameObject.GetComponent<EnemyMelee>().TakeDamage(damage);
        }
    }

    private void Deactivate() 
    {
        gameObject.SetActive(false);
    }
}
