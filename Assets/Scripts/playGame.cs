using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playGame : MonoBehaviour
{
    public CanvasGroup canvas;
    public Camera mainCamera;
    public Transform castle;

    public void PlayGame(){
		StartCoroutine(FadeMenu());
	}

    IEnumerator FadeMenu(){
        // while(camera.EulerAngles.x < 35){
        while(canvas.alpha > 0){
            canvas.alpha -= 0.05f;
            foreach(Transform child in castle){
                Material[] mats = GetComponent<Renderer>().materials;
                foreach(Material mat in mats){

                }
            }

            yield return null;
        }
    }
}

