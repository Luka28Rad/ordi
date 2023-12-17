using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float attractionForce = 10f; // Snaga privlačenja
    public float attractionRadius = 5f; // Radijus privlačenja
    public float boostForce = 20f; // Snaga boosta
    public float boostAngle = 180f; // Kut u stupnjevima unutar kojeg će se dodijeliti boost (polukrug)

    void FixedUpdate()
    {
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
