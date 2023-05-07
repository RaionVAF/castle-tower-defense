using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cryController : MonoBehaviour
{
    public int numZombies;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        numZombies = 0;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(playZombie());
        StartCoroutine(stopZombie());
    }

    IEnumerator playZombie()
    {
        WaitUntil zombiesEntered = new WaitUntil(() => numZombies > 0);
        yield return zombiesEntered;
        audioSource.loop = true;
        audioSource.Play();
    }

    IEnumerator stopZombie()
    {
        WaitUntil zombiesExited = new WaitUntil(() => numZombies < 1);
        yield return zombiesExited;
        audioSource.Stop();
    }
}

