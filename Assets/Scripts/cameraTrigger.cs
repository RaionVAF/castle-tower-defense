using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrigger : MonoBehaviour
{
    public GameObject bounds;
    private RotateCamera rotateCam;

    void Start(){
        rotateCam = bounds.GetComponent<RotateCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "playableKnight"){            
            rotateCam.callCam(int.Parse(transform.name) - 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // play error sound
        // Debug.Log("error");
    }
}
