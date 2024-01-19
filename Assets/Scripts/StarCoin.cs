using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCoin : MonoBehaviour
{
    public float rotationSpeed = 150f;  
    public float movementRange = 0.35f; 
    public float movementSpeed = 4f;

    private float initialY;  

    // Start is called before the first frame update
    void Start()
    {
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
                Destroy(gameObject);
                Debug.Log("Collected " + gameObject.name);
            } else {
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
            Destroy(gameObject);
            Debug.Log("Collected " + gameObject.name);
            }
        }
    }
}
