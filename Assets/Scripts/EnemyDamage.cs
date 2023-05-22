using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collission)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerCombat>.TakeDamage(damage);
        }
    }

}
