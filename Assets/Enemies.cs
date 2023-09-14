using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float movementSpeed = 3f;
    public bool IsGoingRight = true;
    public float raycastingDistance = 1f;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.flipX = IsGoingRight;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionTranslation = (IsGoingRight) ? transform.right : -transform.right;
        directionTranslation *= Time.deltaTime * movementSpeed;

        transform.Translate(directionTranslation);
        CheckForWalls();
    }

    private void CheckForWalls()
    {
        Vector3 raycastingDirection = (IsGoingRight) ? Vector3.right : Vector3.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + raycastingDirection * raycastingDistance - new Vector3(0f, .25f, 0f), raycastingDirection, 0.075f);

        if(hit.collider != null)
        {
            if(hit.transform.tag == "Ground")
            {
            IsGoingRight = !IsGoingRight;
            sprite.flipX = !IsGoingRight;
            }
        }
    }
}
