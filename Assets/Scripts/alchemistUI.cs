using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class alchemistUI : MonoBehaviour
{
    private IEnumerator coroutine;
    public TextMeshProUGUI dialogue;
    public alchemistController alchemistController;

    private string[] script = new string[]{
            "What can I fix for you?",
            "I shall do it. Glory to the kingdom!",
    };
    
    void OnEnable()
    {
        coroutine = speak(script[0]);
        StartCoroutine(coroutine);
    }

    IEnumerator speak(string str){
        alchemistController.speaking(true);

        for(int i = 0; i <= str.Length; i++){
            dialogue.text = str.Substring(0, i);

            yield return null;
        }

        alchemistController.speaking(false);

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
