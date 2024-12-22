using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Nestaskomp : MonoBehaviour
{
    private bool isTileActive = true;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player") && isTileActive)
    {
        // Skip if it's Dusko character
        if(collision.gameObject.GetComponent<SpriteRenderer>().sprite.name.ToLower().Contains("dusko")) 
            return;

        // Get the contact point
        ContactPoint2D contact = collision.GetContact(0);
        
        // Check if the contact normal is pointing downward (meaning player is on top)
        if (contact.normal.y < -0.5f)  // Using -0.5f instead of -1f for some tolerance
        {
            Debug.Log("Player standing on platform");
            StartCoroutine(DisappearForSeconds(1f));
            Achievements.UnlockCloudThroughAchievement();
        }
    }
}

    private IEnumerator DisappearForSeconds(float seconds)
    {
        float elapsedTime = 0f;
        Color targetColor = Color.clear;

        while (elapsedTime < seconds)
        {
            spriteRenderer.color = Color.Lerp(Color.white, targetColor, elapsedTime / seconds);
            
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        spriteRenderer.color = targetColor;

        isTileActive = false;
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(1f);

        spriteRenderer.color = Color.white;
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        isTileActive = true;
    }
}
