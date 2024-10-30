using UnityEngine;

public class DangerLine : MonoBehaviour
{
    public float startSpeed = 1f;            // Initial speed of the line
    public float acceleration = 0.1f;        // Rate at which the line speeds up
    public GameObject player;                // Reference to the player object

    private float currentSpeed;              // Current speed of the line

    private void Start()
    {
        // Set initial speed
        currentSpeed = startSpeed;
    }

    private void Update()
    {
        // Move the line upwards
        transform.position += Vector3.up * currentSpeed * Time.deltaTime;

        // Gradually increase the speed
        currentSpeed += acceleration * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the line collides with the player
        if (other.gameObject == player)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        // Add your player death logic here, such as:
        Debug.Log("Player has been killed by the danger line!");
        // Optionally, trigger a respawn, end the game, etc.
    }
}
