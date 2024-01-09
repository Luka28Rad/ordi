using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;
    // Start is called before the first frame update
    void Start()
    {
        checkpointManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkpointManager.ActivateCheckpoint(transform);
        GetComponent<Collider2D>().enabled = false;
    }
}