using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)    //CHECK IF PLAYER
        { 
            Debug.Log(gameObject.transform.name + " auchhh");
            collision.GetComponent<HealthManager>().TakeDamage();
            if(gameObject.transform.name == "BigFire") Achievements.UnlockSvjetlanaDamageAchievement();
            else if(gameObject.transform.name == "SpringshroomDamage") Achievements.UnlockDamageGljivanAchievement();
            else if(gameObject.transform.name.Contains("Matchstick")) Achievements.UnlockBrunoDamageAchievement();
            else if(gameObject.transform.name.Contains("Pew")) Achievements.UnlockHurtWizardAchievement();
            else if(gameObject.transform.name.Contains("Spider")) Debug.Log("spuderman");//Achievements.UnlockHurtWizardAchievement();
            //Napravi da player bude imun na damge neko kratko vrijeme dodaj coroutine u damage the player
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 0);
        }
    }

    
}
