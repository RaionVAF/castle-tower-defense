using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private Vector3 rotationRate = new Vector3(0f, 90f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
            transform.Rotate(rotationRate * Time.deltaTime, Space.Self);
        } else if(Input.GetKey(KeyCode.LeftArrow)){
            transform.Rotate(- rotationRate * Time.deltaTime, Space.Self);
        }
    }
}
