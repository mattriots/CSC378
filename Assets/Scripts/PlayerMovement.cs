using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;  
    private AudioSource sound;
    public float jumpRate = 90f;
    float nextJumpTime = 0f;

    public Animator animator;


    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // sound = GetComponent<AudioSource>();
    }

    private void Update() {

        float horizontalInput = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // Set player's velocity based no player input
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (Time.time >= nextJumpTime) {
            if (Input.GetKey(KeyCode.Space)) 
            {
            body.velocity = new Vector2(body.velocity.x, speed - 5);
            nextJumpTime = Time.time + 70f / jumpRate;
            // sound.Play();
            } 
        }
        

        // Change the sprite depending on the player's movement direction
        if (horizontalInput > 0) {
            spriteRenderer.flipX = false; // The sprite faces right by default
        } else if (horizontalInput < 0) {
            spriteRenderer.flipX = true; // Flip the sprite to face left
        }

    }
}
