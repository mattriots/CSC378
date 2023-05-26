using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{

    private BoxCollider2D collider;
    [SerializeField] public PlayerCombat player;

    private bool taken;
    // Start is called before the first frame update
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
            taken = true;
            gameObject.SetActive(false);
            // playerHealth = collider.GetComponent<PlayerCombat>();
            player.addShuriken();
        }
    }

    private void Deactivate() 
    {
        gameObject.SetActive(false);
    }
}
