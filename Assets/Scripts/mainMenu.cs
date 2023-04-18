using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    public GameObject canvas;
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
		// StartCoroutine(FadeMenu());
        Destroy(canvas, 0.25f);
	}
}
