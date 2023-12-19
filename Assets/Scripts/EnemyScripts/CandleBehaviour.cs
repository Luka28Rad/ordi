using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleBehaviour : MonoBehaviour
{
    [SerializeField] GameObject bigFire;
    [SerializeField] GameObject smallFire;
    [SerializeField] Animation fireAnimation;
    // Start is called before the first frame update
    void Start()
    {
        bigFire.SetActive(false);
        smallFire.SetActive(true);
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        bigFire.SetActive(false);
        smallFire.SetActive(true);
        fireAnimation.Play();
        yield return new WaitForSeconds(2.5f);  //small fire duration
        bigFire.SetActive(true);
        smallFire.SetActive(false);
        fireAnimation.Play();
        yield return new WaitForSeconds(4f);    //big fire duration
        StartCoroutine(Fire());
    }

    public void Death()
    {
        Destroy(gameObject, 3);
        bigFire.SetActive(false);
        smallFire.SetActive(false);
        StopAllCoroutines();
    }
}
