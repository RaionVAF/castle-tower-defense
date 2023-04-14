using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // private Vector3 rotationRate = new Vector3(0f, 90f, 0f);
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(".")){
            StartCoroutine(Camera(-60f));
        } else if(Input.GetKey(",")){
            StartCoroutine(Camera(60f));
        }
    }

    IEnumerator Camera(float degree){
        Quaternion start1 = transform.localRotation; 
        Quaternion end1 = Quaternion.Euler(0f, transform.localEulerAngles.y + degree, 0f);
        float t = 0;

        while(t < 1){
            t += .05f;
            transform.localRotation = Quaternion.Slerp(start1, end1, t);

            yield return null;
        }
    }
}
