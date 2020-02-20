using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroChunk : MonoBehaviour
{
    Animation anim;
    public GameObject mainMenuChunk;
    private void Start()
    {
        anim = GetComponent<Animation>();
       
    }
    private void Update()
    {
        if (!anim.isPlaying)
        {
            gameObject.SetActive(false);
            mainMenuChunk.SetActive(true);
        }
    }
}
