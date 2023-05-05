using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class blacksmithUI : MonoBehaviour
{
    private IEnumerator coroutine;
    public TextMeshProUGUI dialogue;
    public blacksmithController blacksmithController;

    private string[] script = new string[]{
            "What upgrades do you require?",
            "Good choice, knight.",
    };
    
    void OnEnable()
    {
        coroutine = speak(script[0]);
        StartCoroutine(coroutine);
    }

    IEnumerator speak(string str){
        blacksmithController.speaking(true);

        for(int i = 0; i <= str.Length; i++){
            dialogue.text = str.Substring(0, i);

            yield return null;
        }

        blacksmithController.speaking(false);

        if(str == script[1]){
            yield return new WaitForSeconds(1f);
            coroutine = speak(script[0]);
            StartCoroutine(coroutine);
        }
    }

    public void press(){
        StopCoroutine(coroutine);
        coroutine = speak(script[1]);
        StartCoroutine(coroutine);            
    }
}
