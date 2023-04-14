using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject bsCam;
    public GameObject alCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam.SetActive(true);
        bsCam.SetActive(false);
        alCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
