using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource audioSource;
    public AudioClip hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(hoverSound);
    }
}
