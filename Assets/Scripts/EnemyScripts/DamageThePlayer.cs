using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)    //CHECK IF PLAYER
        { 
            collision.GetComponent<HealthManager>().TakeDamage();
            //Napravi da player bude imun na damge neko kratko vrijeme dodaj coroutine u damage the player
        }
    }

    
}
