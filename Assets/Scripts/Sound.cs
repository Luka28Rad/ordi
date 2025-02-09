using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound : MonoBehaviour
{
    public AudioClip defaultClip; // Add a default audio clip
    public AudioClip level1Clip;
    public AudioClip level2Clip;

    public AudioClip level3Clip;
    public AudioClip bossFightClip;
    public AudioClip clip100;
    public AudioClip clip75;
    public AudioClip clip125;
    public AudioClip clip150;
    // Add more audio clips as needed
    private GameObject deathline;
    private AudioSource audioSource;

    void Start()
    {
        isEndlessLevel = SceneManager.GetActiveScene().name == "EndlessLevel";
        
        if (isEndlessLevel)
        {
            deathline = GameObject.Find("DeathLiine");
            if (deathline == null)
            {
                Debug.LogWarning("DeathLiine not found in Endless Level!");
                return;
            }
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        PlayAudioBasedOnScene();
    }

    void PlayAudioBasedOnScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Level 1":
                PlayAudio(level1Clip);
                break;
            case "Level 2":
                PlayAudio(level2Clip);
                break;
            case "Level 3":
                PlayAudio(level3Clip);
                break;
            case "Bossfight":
                PlayAudio(bossFightClip);
                break;
            case "CreditsScene":
                PlayAudio(defaultClip);
                break;
            case "EndlessLevel":
                break;
            default:
                PlayAudio(defaultClip);
                break;
        }
    }
    private AudioClip currentClip;

    private float checkInterval = 0.5f;
    private float nextCheckTime;
    private bool isEndlessLevel;
    private float lastHeight = float.MinValue;
    private void Update()
    {
        if (!isEndlessLevel) return;
        if (deathline == null) return;
        if (Time.time < nextCheckTime) return;

        nextCheckTime = Time.time + checkInterval;
        
        float currentHeight = deathline.transform.position.y;
        
        // Only check for music change if height changed significantly
        if (Mathf.Abs(currentHeight - lastHeight) < heightThreshold) return;
        
        lastHeight = currentHeight;
        UpdateEndlessLevelMusic(currentHeight);
    }
    [SerializeField] private float heightThreshold = 30f;
    void PlayAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip is missing!");
        }
    }
    private bool isTransitioning = false;

    private void UpdateEndlessLevelMusic(float height)
    {
        AudioClip newClip = null;

        // Using else-if to avoid multiple checks
        if (height < 90f)
        {
            newClip = clip75;
        }
        else if (height < 190f)
        {
            newClip = clip100;
        }
        else if (height < 270f)
        {
            newClip = clip125;
        }
        else
        {
            newClip = clip150;
        }

        // Only change and play audio if it's different from current and not already transitioning
        if (newClip != currentClip && !isTransitioning)
        {
            StartCoroutine(WaitForSongComplete(newClip));
        }
    }

    private IEnumerator WaitForSongComplete(AudioClip newClip)
    {
        isTransitioning = true;

        // If there's a current song playing, wait for it to finish
        if (audioSource.isPlaying)
        {
            // Calculate remaining time in the current clip
            float remainingTime = audioSource.clip.length - audioSource.time;
            
            // Optional: If you want to wait for the next loop point or measure
            // You can round up to the nearest beat/measure here
            
            yield return new WaitForSeconds(remainingTime);
        }

        // Play the new clip
        PlayAudio(newClip);
        currentClip = newClip;
        isTransitioning = false;
    }
}

