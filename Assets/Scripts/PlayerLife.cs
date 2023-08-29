using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))

    }

}
