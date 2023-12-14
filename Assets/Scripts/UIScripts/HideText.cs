using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideText : MonoBehaviour
{
    Animation anim;
    private void Start()
    {
        anim = GetComponent<Animation>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(DestroyText());
    }

    IEnumerator DestroyText()
    {
        anim.Play();
        yield return new WaitForSecondsRealtime(1);
        gameObject.SetActive(false);
    }
}
