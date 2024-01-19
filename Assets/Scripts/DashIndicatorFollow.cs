using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicatorFollow : MonoBehaviour
{
    GameObject player;
    float speed = 11f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 30)
        {
            transform.position = player.transform.position + new Vector3(0.7f, 0.4f, 0);
        }

        if (player.transform.localScale.x < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + new Vector3(0.7f, 0.4f, 0), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + new Vector3(-0.7f, 0.4f, 0), speed * Time.deltaTime);
        }
        
    }
}
