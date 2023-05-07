using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform pivot;
    private int state;
    private bool rotating;
    public GameObject[] healthBars = new GameObject[3];
    private float[] rots = new float[] {60f, 0f, -60f};
    
    // Start is called before the first frame update
    void Start()
    {
        rotating = false;
    }

    public void callCam(int bound){
        if(!rotating && bound != state){
            rotating = true;
            state = bound;
            
            for(int i = 0; i < healthBars.Length; i++) healthBars[i].SetActive(false);

            StartCoroutine(RotateCam(rots[state]));
        }
    }

    IEnumerator RotateCam(float degree){
        Quaternion start1 = pivot.localRotation; 
        Quaternion end1 = Quaternion.Euler(0f, degree, 0f);
        float t = 0;

        while(t < 1){
            t += .05f;
            pivot.localRotation = Quaternion.Lerp(start1, end1, t);

            yield return null;
        }

        healthBars[state].SetActive(true);
        rotating = false;
    }
}
