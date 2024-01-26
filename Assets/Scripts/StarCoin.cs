using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCoin : MonoBehaviour
{
    public float rotationSpeed = 150f;  
    public float movementRange = 0.35f; 
    public float movementSpeed = 4f;
    private AudioSource audioSource;
    private float initialY;  

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        float verticalMovement = Mathf.Sin(Time.time * movementSpeed) * movementRange;
        transform.position = new Vector3(transform.position.x, initialY + verticalMovement, transform.position.z);
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string collectibleName = gameObject.name;

            if(Variables.gameMode == "Speedrun" || Variables.gameMode == "Practice") {
                Variables.speedRunCollectiblesCounter++;
                Collectibles.collectibleCounter++;

            if (PlayerPrefs.HasKey("collectiblesSpeedRun"))
            {
                string currentCollectibles = PlayerPrefs.GetString("collectiblesSpeedRun");
                    currentCollectibles += "," + collectibleName;
                    PlayerPrefs.SetString("collectiblesSpeedRun", currentCollectibles);
            }
            else
            {
                PlayerPrefs.SetString("collectiblesSpeedRun", collectibleName);
            }

                if (!audioSource.isPlaying)
                    {
                        GetComponent<BoxCollider2D>().enabled = false;
                        audioSource.Play();
                        Color currentColor = GetComponent<Renderer>().material.color;
                        currentColor.a = 0f;
                        GetComponent<Renderer>().material.color =currentColor;
                        Destroy(gameObject, audioSource.clip.length);
                    }
                Debug.Log("Collected " + gameObject.name);
            } else if(Variables.gameMode == "NewGame" || Variables.gameMode == "LoadGame") {
            if (PlayerPrefs.HasKey("collectibles"))
            {
                string currentCollectibles = PlayerPrefs.GetString("collectibles");
                    currentCollectibles += "," + collectibleName;
                    PlayerPrefs.SetString("collectibles", currentCollectibles);
            }
            else
            {
                PlayerPrefs.SetString("collectibles", collectibleName);
            }
            Collectibles.collectibleCounter++;
                if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                        GetComponent<BoxCollider2D>().enabled = false;
                        Color currentColor = GetComponent<Renderer>().material.color;
                        currentColor.a = 0f;
                        GetComponent<Renderer>().material.color =currentColor;
                        Destroy(gameObject, audioSource.clip.length);
                    }
            Debug.Log("Collected " + gameObject.name);
            } else {
                                if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                        GetComponent<BoxCollider2D>().enabled = false;
                        Color currentColor = GetComponent<Renderer>().material.color;
                        currentColor.a = 0f;
                        GetComponent<Renderer>().material.color =currentColor;
                        Destroy(gameObject, audioSource.clip.length);
                    }
                Debug.Log("Just the sound.");
            }
        }
    }
}
