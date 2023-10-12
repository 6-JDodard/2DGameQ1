using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletPosition;  
    private GameObject Player;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(transform.position, Player.transform.position);
        

        if(distance < 10)
        {
            timer += Time.deltaTime;

            if(timer > 2)
            {
                timer = 0;
                shoot();
            }
        }

      
    }

    void shoot()
    {
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);

    }
}
