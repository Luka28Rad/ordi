using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public void HasBeenFired()
    {
        Destroy(gameObject, 5);
    }
}
