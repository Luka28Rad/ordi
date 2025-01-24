using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicBook : MonoBehaviour
{
    public List<Sprite> episodes = new List<Sprite>();
    public Image episodeImage;
    private int epCounter = 1;
    public GameObject particleHolder;
    public AudioSource audioSource;
    void Start()
    {
        epCounter = 1;
        audioSource.Play();
        ShowComicBookEpisode();
    }

    public void LeftPressed(){
        epCounter--;
        if(epCounter < 1){
            epCounter = 1;
        } else {
            audioSource.Play();
        }
        ShowComicBookEpisode();
    }

    public void RightPressed(){
        epCounter++;
        if(epCounter > 12){
            epCounter = 12;   
        } else {
            audioSource.Play();
        }
        ShowComicBookEpisode();
    }

    private void ShowComicBookEpisode(){
        episodeImage.sprite = episodes[epCounter-1];
        Vector3 newScale = new Vector3(0.3f, 0.3f, 0.3f);
        episodeImage.transform.localScale = newScale;
        if(epCounter==12 || (epCounter > 0 && epCounter < 3)) ActivateParticles(true);
        else if(epCounter >= 3) ActivateParticles(false);

        if(epCounter == 1) Achievements.UnlockFirstEpisodeAchievement();
        else if(epCounter == 6) Achievements.UnlockHalfComicAchievement();
        else if(epCounter == 12) Achievements.UnlockReadComicAchievement();
    }

    private void ActivateParticles(bool state){
        foreach (Transform child in particleHolder.transform) {
            if(epCounter != 3) child.gameObject.SetActive(state);
            else child.gameObject.SetActive(true);
            ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            var mainModule = particleSystem.main;
            mainModule.loop = state;
        }
    }
}
