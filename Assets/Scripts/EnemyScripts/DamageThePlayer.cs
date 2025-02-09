using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThePlayer : MonoBehaviour
{
    private bool yellow = false, green = false, red = false, black = false;
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
            else if(gameObject.transform.name.Contains("BlackSpider") || gameObject.transform.name.Contains("YellowSpider")) {
                Debug.Log("spuderman");
                Achievements.UnlockBlackYellowSpiderDamage();
                if (gameObject.transform.name.Contains("BlackSpider")) black = true;
                if (gameObject.transform.name.Contains("YellowSpider")) yellow = true;
                if(red && green && black && yellow) Achievements.UnlockSurviveFourBites();
            }
            else if(gameObject.transform.name.Contains("RedSpider") || gameObject.transform.name.Contains("GreenSpider")){
                Debug.Log("spuderman");
                Achievements.UnlockRedGreenSpiderDamage();
                if (gameObject.transform.name.Contains("RedSpider")) red = true;
                if (gameObject.transform.name.Contains("GreenSpider")) green = true;
                if(red && green && black && yellow) Achievements.UnlockSurviveFourBites();
            }
             
            //Napravi da player bude imun na damge neko kratko vrijeme dodaj coroutine u damage the player
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 0);
        }
    }

    
}
