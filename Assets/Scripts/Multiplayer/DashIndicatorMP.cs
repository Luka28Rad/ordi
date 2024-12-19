using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class DashIndicatorMP : NetworkBehaviour
{
    GameObject player;
    float speed = 11f;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && Vector2.Distance(player.transform.position, transform.position) > 30)
        {
            transform.position = player.transform.position + new Vector3(0.7f, 0.4f, 0);
        }

        if (player != null && player.transform.localScale.x < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + new Vector3(0.7f, 0.4f, 0), speed * Time.deltaTime);
        }
        else if(player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + new Vector3(-0.7f, 0.4f, 0), speed * Time.deltaTime);
        }
    }
}
