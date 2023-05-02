using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public GameObject mainCam, bsCam, alCam, player;
    public bool followcam;
    float m_FieldOfView;
    // Start is called before the first frame update
    void Start()
    {
        mainCam.SetActive(true);
        bsCam.SetActive(false);
        alCam.SetActive(false);
        m_FieldOfView = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (followcam){
            transform.position = player.transform.position + new Vector3(0, 50f, 50f);
            Camera.main.fieldOfView = m_FieldOfView;
        }
    }
    void OnGUI()
    {
        //Set up the maximum and minimum values the Slider can return (you can change these)
        float max, min;
        max = 150.0f;
        min = 20.0f;
        //This Slider changes the field of view of the Camera between the minimum and maximum values
        m_FieldOfView = GUI.HorizontalSlider(new Rect(20, 20, 100, 40), m_FieldOfView, min, max);
    }
}
