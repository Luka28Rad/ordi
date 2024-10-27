using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float attractionForce = 10f; // Snaga privlačenja
    public float attractionRadius = 5f; // Radijus privlačenja
    public float boostForce = 20f; // Snaga boosta
    public float boostAngle = 180f; // Kut u stupnjevima unutar kojeg će se dodijeliti boost (polukrug)
    GameObject player;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private bool IsPlayerClose()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance <= 5f) {
                Achievements.UnlockSpotBlackHoleAchievement();
                return true;
            }
        }
        return false;
    }
    void FixedUpdate()
    {
        if (IsPlayerClose())
        {
            Debug.Log("Player is close to the enemy!");
        } 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 forceDirection = transform.position - collider.transform.position;

                    // Primijeni gravitacijsku silu za privlačenje igrača
                    rb.AddForce(attractionForce * forceDirection.normalized);
                    Achievements.UnlockEnterBlackHoleAchievement();
                    // Odredi kut između igrača i središta crne rupe
                    float angle = Vector2.Angle(Vector2.up, forceDirection);

                    // Ako kut premašuje pola crne rupe, dodaj boost
                    if (angle <= boostAngle / 2f)
                    {
                        rb.AddForce(boostForce * forceDirection.normalized);
                    }
                }
            }
        }
    }
}
