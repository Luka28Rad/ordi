using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelection : MonoBehaviour
{
    public void OnButtonPressed()
    {
        // Perform the button action here
        Debug.Log("Button Pressed!");

        // Deselect the button
        EventSystem.current.SetSelectedGameObject(null);
    }
}
