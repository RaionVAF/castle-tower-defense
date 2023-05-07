using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrigger : MonoBehaviour
{
    public GameObject bounds;
    private RotateCamera rotateCam;
    public AudioSource audioSource;
    public AudioClip error;

    void Start(){
        rotateCam = bounds.GetComponent<RotateCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "playableKnight"){ 
            // print(int.Parse(transform.name) - 1);     
            rotateCam.callCam(int.Parse(transform.name) - 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // play error sound
        audioSource.PlayOneShot(error, 1f);
        // Debug.Log("error");
    }
}
