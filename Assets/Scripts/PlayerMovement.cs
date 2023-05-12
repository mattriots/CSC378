using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;  
    private AudioSource sound;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    // public float jumpRate = 90f;
    // float nextJumpTime = 0f;

    public Animator animator;
    public bool jump = false;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        // sound = GetComponent<AudioSource>();
    }

    // public void OnLanding() {
    //     animator.SetBool("IsJumping", false);
    //     jump = false;
    // }

    private bool IsGrounded() 
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Update() {

        float horizontalInput = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // Set player's velocity based no player input
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (Input.GetKey(KeyCode.Space) && IsGrounded()) 
        {
            body.velocity = new Vector2(body.velocity.x, speed - 1);
            // jump = true;
            // animator.SetBool("IsJumping", true);
            // nextJumpTime = Time.time + 70f / jumpRate;
            // sound.Play();
        } 
        
        

        // Change the sprite depending on the player's movement direction
        if (horizontalInput > 0) {
            spriteRenderer.flipX = false; // The sprite faces right by default
        } else if (horizontalInput < 0) {
            spriteRenderer.flipX = true; // Flip the sprite to face left
        }

    }
}
