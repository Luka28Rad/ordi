using UnityEngine;
using System.Collections;

public class Dissapear : MonoBehaviour
{
    public float fadeDuration = 2f; // Adjust the duration of each fade
    public float delayBetweenFades = 1f; // Adjust the delay between fades

    private Material material;
    private Color originalColor;

    void Start()
    {
        // Get the material of the object
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalColor = material.color;

            // Start the continuous fading loop
            if (gameObject.name == "Nestasko")StartCoroutine(ContinuousFadeLoop());
        }
        else
        {
            Debug.LogError("Renderer component not found on the object.");
        }
    }

        public float spinSpeed = 180f; // Adjust the speed of the spin

    void Update()
    {
       if(gameObject.name == "StarCoin") transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    IEnumerator ContinuousFadeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeOut());
            yield return new WaitForSeconds(delayBetweenFades);
            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(delayBetweenFades);
        }
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, originalColor.a, elapsedTime / fadeDuration);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
