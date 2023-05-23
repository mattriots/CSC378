using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{

    private BoxCollider2D collider;
    public PlayerCombat playerHealth;
    [SerializeField] private int healAmount;

    private bool taken;


    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (taken)
        {
            return;
        }   
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Heal player");
            taken = true;
            gameObject.SetActive(false);
            // playerHealth = collider.GetComponent<PlayerCombat>();
            playerHealth.healHealth(healAmount);
        }
    }

    private void Deactivate() 
    {
        gameObject.SetActive(false);
    }
}
