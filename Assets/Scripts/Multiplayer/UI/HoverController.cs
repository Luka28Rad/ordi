using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    public TMP_Text charTitle;
    public TMP_Text charName;
    public TMP_Text charAbility;
    public TMP_Text charActivation;
    public TMP_Text charRefresh;
    public TMP_Text charDuration;

    // Called when the pointer enters the object
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the panel when hovering over the object
        panel.SetActive(true);
        SetText();
    }

    // Called when the pointer exits the object
    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the panel when the pointer exits
        panel.SetActive(false);
    }

    private void SetText(){
        string name = charTitle.text;
        if(name == "Zvjezdan") {
            charName.text = "NAME: " + "Zvjezdan";
            charAbility.text = "ABILITY: " +"Dash";
            charActivation.text = "ACTIVATION: " +"Shift";
            charDuration.text = "DURATION: " +"Instant";
            charRefresh.text = "REFRESH: " +"1.5s";
        } else if(name == "Duško") {
            charName.text = "NAME " +"Duško";
            charAbility.text = "ABILITY: " +"Fly";
            charActivation.text = "ACTIVATION: " +"Hold jump";
            charDuration.text = "DURATION: " +"Instant";
            charRefresh.text = "REFRESH: " +"4s";
        } else if(name == "Darko") {
            charName.text = "NAME: " + "Darko";
            charAbility.text = "ABILITY: " +"Teleport";
            charActivation.text = "ACTIVATION: " +"Shift";
            charDuration.text = "DURATION: " +"Instant";
            charRefresh.text = "REFRESH: " +"10s";
        } else if(name == "Gljivan") {
            charName.text = "NAME: " + "Gljivan";
            charAbility.text = "ABILITY: " +"Double jump";
            charActivation.text = "ACTIVATION: " +"Jump button";
            charDuration.text = "DURATION: " +"Instant";
            charRefresh.text = "REFRESH: " +"2s";
        } else if(name == "Svjetlana") {
            charName.text = "NAME: " + "Svjetlana";
            charAbility.text = "ABILITY: " +"Jump boost";
            charActivation.text = "ACTIVATION: " +"Shift";
            charDuration.text = "DURATION: " +"5s";
            charRefresh.text = "REFRESH: " +"10s";
        } else if(name == "Bruno") {
            charName.text = "NAME: " + "Bruno";
            charAbility.text = "ABILITY: " + "Jump boost";
            charActivation.text = "ACTIVATION: " +"Shift";
            charDuration.text = "DURATION: " +"5s";
            charRefresh.text = "REFRESH: " +"10s";
        }
    }
}
