using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FlameFlicker : MonoBehaviour
{
    Light2D lightFlame;
    [SerializeField] int framesToDivide = 3;
    int frames;
    // Start is called before the first frame update
    void Start()
    {
        lightFlame = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frames++;
        if (frames % framesToDivide == 0)
        {
            frames = 0;
            Frame10Update();
        }
        
    }
    void Frame10Update()
    {
        lightFlame.intensity = Random.Range(2.0f, 4.0f);
        transform.eulerAngles = new Vector3(0, Random.Range(-5.0f, 5.0f), Random.Range(-10.0f, 10.0f));
        transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), 1, 1);
    }
}
