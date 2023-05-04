using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonClick : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public void playSound()
    {
        source.PlayOneShot(clip);
    }
}
