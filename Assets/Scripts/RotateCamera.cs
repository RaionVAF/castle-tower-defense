using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform pivot;
    private bool[] sides;
    private bool rotating;
    
    // Start is called before the first frame update
    void Start()
    {
        sides = new bool[] {false, true};
        rotating = false;
    }

    public void callCam(int bound){
        if(bound > 1) return;

        if(!rotating){
            rotating = true;
            
            if(sides[bound]){
                StartCoroutine(RotateCam(-60f));
            } else {
                StartCoroutine(RotateCam(60f));
            }
        }
    }

    IEnumerator RotateCam(float degree){
        Quaternion start1 = pivot.localRotation; 
        Quaternion end1 = Quaternion.Euler(0f, pivot.localEulerAngles.y + degree, 0f);
        float t = 0;

        while(t < 1){
            t += .05f;
            pivot.localRotation = Quaternion.Lerp(start1, end1, t);

            yield return null;
        }

        for(int i = 0; i < sides.Length; i++) sides[i] = !sides[i];

        rotating = false;
    }
}
