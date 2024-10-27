using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleBehaviour : MonoBehaviour
{
    [SerializeField] GameObject bigFire;
    [SerializeField] GameObject smallFire;
    [SerializeField] Animation fireAnimation;

    private AudioSource audioSource;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bigFire.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        smallFire.SetActive(true);
        StartCoroutine(Fire());
    }

    void Update(){
        if (IsPlayerClose())
        {
            Debug.Log("Player is close to the enemy!");
        } 
    }

    private bool IsPlayerClose()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance <= 5f) {
                Achievements.UnlockSpotSvjetlanaAchievement();
                return true;
            }
        }
        return false;
    }
    IEnumerator Fire()
    {
        bigFire.SetActive(false);
        smallFire.SetActive(true);
        fireAnimation.Play();
        yield return new WaitForSeconds(2.5f);  //small fire duration
        bigFire.SetActive(true);
        audioSource.Play();
        smallFire.SetActive(false);
        fireAnimation.Play();
        yield return new WaitForSeconds(4f);    //big fire duration
        StartCoroutine(Fire());
    }

    public void Death()
    {
        Achievements.UnlockSvjetlanaKillAchievement();
        Destroy(gameObject, 3);
        bigFire.SetActive(false);
        smallFire.SetActive(false);
        StopAllCoroutines();
    }
}
