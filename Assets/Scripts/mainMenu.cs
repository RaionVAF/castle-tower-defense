using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject playerUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(0f, 1f, 0f);
    }

    public void PlayGame(){
        menu.SetActive(false);
        playerUI.SetActive(true);
	}

    public void QuitGame(){
        Application.Quit();
	}
}
