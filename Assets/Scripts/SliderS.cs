using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderS : MonoBehaviour
{
    public Slider sliderSound;
    private float musicVolume;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
                audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        musicVolume = PlayerPrefs.GetFloat("Volume", 1f);
        sliderSound.value = musicVolume;
        sliderSound.onValueChanged.AddListener(VolumeUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = musicVolume;
        PlayerPrefs.SetFloat("Volume", musicVolume);
    }

    public void VolumeUpdate(float volume) {
        musicVolume = volume;
    }
}
