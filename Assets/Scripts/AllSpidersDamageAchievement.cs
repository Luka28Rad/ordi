using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSpidersDamageAchievement : MonoBehaviour
{
    private bool yellow = false, green = false, red = false, black = false;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.transform.name.Contains("BlackSpider") || collision.transform.name.Contains("YellowSpider")) {
                if (collision.transform.name.Contains("BlackSpider")) black = true;
                if (collision.transform.name.Contains("YellowSpider")) yellow = true;
                if(red && green && black && yellow) {
                    Achievements.UnlockSurviveFourBites();
                    Debug.Log("All bites are here");
                }
            }
            else if(collision.transform.name.Contains("RedSpider") || collision.transform.name.Contains("GreenSpider")){
                Debug.Log("spuderman");
                Achievements.UnlockRedGreenSpiderDamage();
                if (collision.transform.name.Contains("RedSpider")) red = true;
                if (collision.transform.name.Contains("GreenSpider")) green = true;
                if(red && green && black && yellow) {
                    Achievements.UnlockSurviveFourBites();
                    Debug.Log("All bites are here");
                }
            }
    }
}
