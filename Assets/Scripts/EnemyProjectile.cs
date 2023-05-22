using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{

    [SerializeField] private float speed;
    [SerializeField] private float resetTime;

    private float lifetime;
    private Animator animator;
    private BoxCollider2D collider;
    private bool hit;


    private void Awake() 
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        GameObject.SetActive(true);
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
            GameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        hit = true;
        base.OnTriggerEnter2D(collision);
        collision.enabled = false;

        Debug.log("Hit with Projectile");

        if (animator != null) 
        {

        } else {
            GameObject.SetActive(false);
        }
 
    }

    private void Deactivate() 
    {
        GameObject.SetActive(false);
    }
}
