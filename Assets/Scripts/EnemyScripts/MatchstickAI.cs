using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchstickAI : MonoBehaviour
{
    [SerializeField] Sprite ugasen;
    SpriteRenderer renderer;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject smoke;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        smoke.SetActive(false);
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            animator.enabled = false;
            renderer.sprite = ugasen;
            fire.SetActive(false);
            smoke.SetActive(true);
        }
    }
}
