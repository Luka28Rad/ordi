using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    readonly List<Vector2> checkpoints = new();

    private void Start()
    {
        checkpoints.Add(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    public void ActivateCheckpoint(Transform transformOfCheckpoint)
    {
        checkpoints.Add(transformOfCheckpoint.position);
    }

    public Vector2 GetLastCheckpoint()
    {
        return checkpoints.Last();
    }
}
