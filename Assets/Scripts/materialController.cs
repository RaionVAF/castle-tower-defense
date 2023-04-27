using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialController : MonoBehaviour
{
    private float t = 0;
    private bool flip = true;
    private Vector3 start;
    private Vector3 end;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        end = transform.position + new Vector3(0f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0f, 2f, 0f);
        
        transform.position = Vector3.Lerp(start, end, t);

        // print(t + " = " + transform.position);
        
        if(flip){
            t += .025f;

            if(t >= 1){
                flip = false;
            }
        } else {
            t -= .025f;

            if(t <= 0){
                flip = true;
            }
        }
    }
}
