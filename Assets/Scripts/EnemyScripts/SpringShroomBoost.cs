using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringShroomBoost : MonoBehaviour
{
    private float jumpStrength = 22f;
    public AudioClip[] jumpSoundEffects;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpStrength;
        }
    }
}
