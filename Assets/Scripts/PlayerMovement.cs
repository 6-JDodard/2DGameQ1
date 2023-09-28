using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool doubleJump;
    public float dashSpeed;

    public float dashLength = .5f, dashCooldown = 1f;

    private float dashCounter;
    private float dashCoolCounter;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask jumpableGround;
    private float dirX = 0f;

    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       coll = GetComponent<BoxCollider2D>();
       sprite = GetComponent<SpriteRenderer>();
       anim = GetComponent<Animator>();

       
    }

    // Update is called once per frame
    private void Update()
    {
      dirX = Input.GetAxisRaw("Horizontal");

       rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

      if(Input.GetButtonDown("Jump") && isGrounded())
      {
        jumpSound.Play();
        rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
      }  
      
      if (Input.GetKeyDown(KeyCode.Space))
      {
        if(dashCoolCounter <=0 && dashCoolCounter <=0)
        {
           moveSpeed = dashSpeed;
           dashCounter = dashLength;
        }
      }

      if (dashCounter > 0)
      {
         dashCounter -= Time.deltaTime;

         if (dashCounter <=0)
         {
           dashCoolCounter = dashCooldown;
         }
      }

      if (dashCoolCounter > 0)
      {
        dashCoolCounter -= Time.deltaTime;
      }
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
      MovementState state;

        if (dirX > 0f)
         {
          state = MovementState.running;
          sprite.flipX = false;
         }
       else if (dirX < 0f)
       {
          state = MovementState.running;
          sprite.flipX = true;
       }
       else
       {
          state = MovementState.idle;
       }

       if(rb.velocity.y > .1f)
       {
         state = MovementState.jumping;
       }
       else if (rb.velocity.y < -.1f)
       {
         state = MovementState.falling;
       }

      anim.SetInteger("state",(int)state);
   }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.CompareTag("Enemy"))
        {
            
            Destroy(collision.gameObject);
        
        }
    }

   
}
