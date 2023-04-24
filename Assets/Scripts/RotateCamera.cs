using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform pivot;
    public GameObject bounds;
    private bool[] sides;
    
    // Start is called before the first frame update
    void Start()
    {
        sides = new bool[] {true, false, true, false, true, false};
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "playableKnight"){
            if(sides[int.Parse(transform.name) - 1]){
                StartCoroutine(Camera(-60f));
            } else {
                StartCoroutine(Camera(60f));
            }
        }
    }

    IEnumerator Camera(float degree){
        Quaternion start1 = pivot.localRotation; 
        Quaternion end1 = Quaternion.Euler(0f, pivot.localEulerAngles.y + degree, 0f);
        float t = 0;

        while(t < 1){
            t += .05f;
            pivot.localRotation = Quaternion.Slerp(start1, end1, t);

            yield return null;
        }

        for(int i = 0; i < sides.Length; i++) sides[i] = !sides[i];
    }
}
