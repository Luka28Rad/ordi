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

    private AudioSource audioSource;

    void Start()
    {
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
                PlayAudio(clip100);
                break;
            default:
                PlayAudio(defaultClip);
                break;
        }
    }

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
}

