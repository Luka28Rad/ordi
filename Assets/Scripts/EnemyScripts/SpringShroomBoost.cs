using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringShroomBoost : MonoBehaviour
{
    private float jumpStrength = 22f;
    public AudioClip[] jumpSoundEffects;
    public AudioSource audioSource;

    private int counter = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int randomIndex = Random.Range(0, jumpSoundEffects.Length);
        AudioClip jumpSound = jumpSoundEffects[randomIndex];
        audioSource.clip = jumpSound;
        if (collision.gameObject.layer != 3)    //CHECK IF NOT PLAYER
        {
            return;
        }

        if (collision.GetComponent<Transform>().transform.position.y > transform.position.y)
        {
            audioSource.Play();
            counter++;
            if(counter == 10) {
                Achievements.UnlockGljivanSoundsAchievement();
            }
            Debug.Log("SKOOCI");
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpStrength;
            Achievements.UnlockHelpGljivanAchievement();
        }
    }
}
