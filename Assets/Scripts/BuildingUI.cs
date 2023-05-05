using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject interactionPopup;

    bool isPlayerDetected = false;

    void Start()
    {
        interactionPopup.SetActive(false);
    }

    void Update()
    {
        if (isPlayerDetected)
        {
            interactionPopup.SetActive(true);
        }
        else
        {
            interactionPopup.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        isPlayerDetected = false;
    }
}