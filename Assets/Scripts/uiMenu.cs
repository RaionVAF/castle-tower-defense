using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
	}
}
