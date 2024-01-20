using System.Collections;
using UnityEngine;

public class Nestasko : MonoBehaviour
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
            Debug.Log("Bok");
            StartCoroutine(DisappearForSeconds(1f));
        }
    }

    private IEnumerator DisappearForSeconds(float seconds)
    {
        float elapsedTime = 0f;
        Color targetColor = Color.red;

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
