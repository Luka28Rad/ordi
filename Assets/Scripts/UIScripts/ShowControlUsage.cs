using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControlUsage : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject[] keys;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            keys[0].transform.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else
        {
            keys[0].transform.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }

        if (Input.GetKey(KeyCode.D))
        {
            keys[1].transform.GetComponent<SpriteRenderer>().sprite = sprites[3];
        }
        else
        {
            keys[1].transform.GetComponent<SpriteRenderer>().sprite = sprites[2];
        }

        if (Input.GetKey(KeyCode.Space))
        {
            keys[2].transform.GetComponent<SpriteRenderer>().sprite = sprites[5];
        }
        else
        {
            keys[2].transform.GetComponent<SpriteRenderer>().sprite = sprites[4];
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            keys[3].transform.GetComponent<SpriteRenderer>().sprite = sprites[7];
        }
        else
        {
            keys[3].transform.GetComponent<SpriteRenderer>().sprite = sprites[6];
        }
    }
}
