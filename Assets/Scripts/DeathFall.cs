using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFall : MonoBehaviour
{
  [SerializeField] private PlayerLife deathScript;
    void Update()
    {
        if(transform.position.y < -10)
        {
          deathScript.RestartLevel();
        }
    }
}
