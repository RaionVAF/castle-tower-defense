using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject resources, xp;
    public GameObject on, off;

    public void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
	}

    public void ToggleOn(){
        resources.SetActive(false);
        xp.SetActive(false);
        on.SetActive(false);
        off.SetActive(true);
    }

    public void ToggleOff(){
        resources.SetActive(true);
        xp.SetActive(true);
        on.SetActive(true);
        off.SetActive(false);
    }
}
