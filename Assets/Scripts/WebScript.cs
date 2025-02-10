using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebScript : MonoBehaviour
{
    public List<GameObject> spiders = new List<GameObject>();
    private bool coroutineActive = false;
    private GameObject currentlyActiveSpider = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !coroutineActive)
        {
            StartCoroutine(ActivateRandomSpider());
        }
    }

    private IEnumerator ActivateRandomSpider()
    {
        // Set flag to prevent multiple coroutines
        coroutineActive = true;

        // Try to activate a random inactive spider
        if (TryActivateRandomInactiveSpider())
        {
            // Wait for 3 seconds
            yield return new WaitForSeconds(5f);

            // Deactivate the spider that was activated
            if (currentlyActiveSpider != null)
            {
                currentlyActiveSpider.SetActive(false);
                currentlyActiveSpider = null;
                Debug.Log("Spider deactivated");
            }
        }

        // Reset flag
        coroutineActive = false;
    }

    private bool TryActivateRandomInactiveSpider()
    {
        // Get list of inactive spiders
        List<GameObject> inactiveSpiders = spiders.FindAll(spider => !spider.activeSelf);

        // If no inactive spiders, return false
        if (inactiveSpiders.Count == 0)
        {
            Debug.Log("All spiders are already active!");
            return false;
        }

        // Pick a random inactive spider
        int randomIndex = Random.Range(0, inactiveSpiders.Count);
        currentlyActiveSpider = inactiveSpiders[randomIndex];

        // Activate the selected spider
        currentlyActiveSpider.SetActive(true);
        Debug.Log($"Activated spider: {currentlyActiveSpider.name}");
        if(inactiveSpiders.Count == 1) {
            Achievements.UnlockEnterAllWebs();
            Debug.Log("Sve mreze!");
        }
        return true;
    }
}