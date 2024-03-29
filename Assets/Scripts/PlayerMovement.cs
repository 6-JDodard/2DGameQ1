using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    float horizontal;
    
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    
   
 
    // Wall Jump Variables
    [Header("Wall Jump System")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] public float wallSlidingSpeed;
    bool isWallTouch;
    bool isSliding;
    
 
    

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask groundLayer;
    private bool IsGrounded;
    

    private float dirX = 0f;
    private enum MovementState { idle, running, jumping, falling,}

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
      isWallTouch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.9f, 1f), 0, groundLayer);

         if(isWallTouch && !isGrounded() && horizontal != 0)  
        { 
          Debug.Log("Slide");
            isSliding = true;
        }
        else
        {
         
            isSliding = false;
        }
    

      if(isDashing)
      {
        return;
      }

      dirX = Input.GetAxisRaw("Horizontal");

       rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


      if(Input.GetButtonDown("Jump") && isGrounded())
      {
        jumpSound.Play();
        rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
      }  



      
      if (isSliding)
      {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
      }

    
      //Dash Ability
      if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
      {
        StartCoroutine(Dash());
      }
       IEnumerator Dash()
      {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
      }
      
      UpdateAnimationState();
    }

    //Animations
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
   private void OnCollisionEnter2D(Collision2D collision)
   {
        if(collision.gameObject.CompareTag("Wall"))
        {
            
            rb.gravityScale = rb.gravityScale / 2;
        
        }
   }
}
