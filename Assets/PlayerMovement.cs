using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask jumpableGround;
    private float dirX = 0f;

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
        rb.velocity = new Vector2 (rb.velocity.x, jumpForce);

       }  

        UpdateAnimationUpdate();
    }
    private void UpdateAnimationUpdate()
    {
        if (dirX > 0f)
       {
          anim.SetBool("Running", true);
       }
       else if (dirX < 0f)
       {
          anim.SetBool("Running", true);
       }
       else
       {
          anim.SetBool("Running", false);
       }

    }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
