using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private float horizontal;
    
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
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
    horizontal = Input.GetAxisRaw("Horizontal");

     bool IsWalled()
      {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
      }

     void WallSlide()
      {
       if (IsWalled() && !isGrounded() && horizontal !=0f)
      {
         isWallSliding = true;
         rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
      } 
      else
      {
        isWallSliding = false;
      }
    }

      WallSlide();

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
