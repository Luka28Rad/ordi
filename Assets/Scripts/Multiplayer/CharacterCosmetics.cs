using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using TMPro;

public class CharacterCosmetics : MonoBehaviour
{
    public int currentColorIndex = 0;
    public Sprite[] playerColors;
    public Image currentColorImage;
    public TMP_Text currentColorText;
    public GameObject LocalPlayerObject;
    public PlayerObjectController LocalplayerController;

    private void Start()
    {
        currentColorIndex = PlayerPrefs.GetInt("currentColorIndex", 0);
        currentColorImage.sprite = playerColors[currentColorIndex];
        currentColorText.text = playerColors[currentColorIndex].name;
        StartCoroutine(DelayedFindLocalPlayer());
    }

    private IEnumerator DelayedFindLocalPlayer()
    {
        // Wait for 0.1 seconds
        yield return new WaitForSeconds(0.1f);

        // Call FindLocalPlayer
        FindLocalPlayer();
    }

    private int count = 0; 
    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        if (LocalPlayerObject != null)
        {
            LocalplayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
            LocalplayerController.CmdUpdatePlayerColor(currentColorIndex); // Automatically apply the last used color.
            Debug.Log("FindLocalPlayer called");
        }
        else
        {
            if(count == 5) return;
            FindLocalPlayer();
            count++;
            Debug.LogError("LocalGamePlayer not found! " + count);
        }
    }

    public void NextColor(){
        FindLocalPlayer();
        if(!(currentColorIndex < playerColors.Length -1)) currentColorIndex = -1;
        if(currentColorIndex < playerColors.Length -1) {
            currentColorIndex++;
            SetColor(currentColorIndex);
        }
    }

    public void PreviousColor(){
        FindLocalPlayer();
        if(!(currentColorIndex > 0)) currentColorIndex = playerColors.Length;
        if(currentColorIndex > 0) {
            currentColorIndex--;
            SetColor(currentColorIndex);
        }
    }

        private void SetColor(int index)
    {
        PlayerPrefs.SetInt("currentColourIndex", currentColorIndex);
        currentColorImage.sprite = playerColors[currentColorIndex];
        currentColorText.text = playerColors[currentColorIndex].name;
        LocalplayerController.CmdUpdatePlayerColor(currentColorIndex);
    }
}