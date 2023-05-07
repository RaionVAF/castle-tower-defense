using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tutorial : MonoBehaviour
{
    public GameObject player;
    public GameObject king;
    public GameObject spawn;

    private characterController playerController;
    private kingController kingController;
    public GameObject healthBars;
    public GameObject cameraBounds;

    public GameObject playerUI;
    public GameObject tutorialUI;
    public GameObject skipButton;
    public TextMeshProUGUI dialogue;

    private IEnumerator coroutine;
    private string[] script = new string[]{
            "Today we are all here to celebrate the 50th anniversary of our great kingdom.",
            "Despite the horrors and difficulties we have faced during that time, we persevered.",
            "Had it not been for the efforts of our people, we would have fallen.",
            "Give a round of applause for our kingdom's glorious knight,",
            "who has provided a valiant effort in order to protect our kingdom from those who wish us harm.",
            "Brave Knight, I humbly ask you protect our kingdom once more.",
            "To move, press WASD to travel in the directions displayed on the screen.",
            "To change regions, walk towards either the top or left of the screen.",
            "To repair a building, talk to the alchemist in that building's region.",
            "To upgrade a building, talk to the blacksmith in that building's region.",
            "Both interactions can be triggered by approaching their respective buildings",
            "and pressing 'E' on your keyboard. You can also leave the interactions by pressing 'E'.",
            "When killed, enemies will drop materials to upgrade our defenses and XP to repair our village.",
            "Simply click on any materials you may find on the ground to collect them.",
            "Wood will upgrade your tower's shooting speed,",
            "stone will upgrade your tower's projectile damage,",
            "and iron will replace the tower's ballista with a cannon.",
            "Fair knight, defend our way of life once more!"
    };
    private int line = 0;

    private float startTime;
    
    public GameObject directionPopup;
    private Quaternion directionRotation;

    public GameObject leftArrow;
    public GameObject upArrow;
    private Vector2 leftArrowPos;
    private Vector2 upArrowPos;
    private RectTransform leftArrowTrans;
    private RectTransform upArrowTrans;

    public GameObject alcSpot;
    public GameObject bsSpot;

    public GameObject materials;
    public materialController woodController;
    public materialController stoneController;
    public materialController ironController;

    void OnEnable()
    {

        playerController = player.GetComponent<characterController>();
        playerController.enabled = false;

        kingController = king.GetComponent<kingController>();

        startTime = Time.time;

        coroutine = speak(script[line++]);
        StartCoroutine(coroutine);

        leftArrowTrans = leftArrow.GetComponent<RectTransform>();
        upArrowTrans = upArrow.GetComponent<RectTransform>();
        leftArrowPos = leftArrowTrans.anchoredPosition;
        upArrowPos = upArrowTrans.anchoredPosition;

        directionRotation = directionPopup.transform.rotation;

        StartCoroutine(BounceArrows());
    }

    void Update(){
        if(line == 2){
            skipButton.SetActive(true);
        }

        if(line == 7){
            playerController.enabled = true;
            directionPopup.SetActive(true);
            directionPopup.transform.rotation = directionRotation;
        }

        if(line == 8){
            // playerController.enabled = false;
            directionPopup.SetActive(false);
            leftArrow.SetActive(true);
            upArrow.SetActive(true);
        }

        if(line == 9){
            alcSpot.SetActive(true);
            leftArrow.SetActive(false);
            upArrow.SetActive(false);
        }

        if(line == 10){
            alcSpot.SetActive(false);
            bsSpot.SetActive(true);
        }

        if(line == 11){
            bsSpot.SetActive(false);
        }

        if(line == 13){
            materials.SetActive(true);
        }

        if(line == 15){
            woodController.enabled = true;
        }

        if(line == 16){
            woodController.enabled = false;
            stoneController.enabled = true;
        }

        if(line == 17){
            stoneController.enabled = false;
            ironController.enabled = true;
        }

        if(line == script.Length){
            materials.SetActive(false);
            skipButton.SetActive(false);
        }
    }

    // There's an error to when the king is speaking ?? idk whats wrong with it
    IEnumerator speak(string str){
        kingController.speaking(true);

        for(int i = 0; i <= str.Length; i++){
            dialogue.text = str.Substring(0, i);

            yield return null;
        }

        kingController.speaking(false);
    }

    public void skip(){
        playerController.enabled = true;
        directionPopup.SetActive(false);
        tutorialUI.SetActive(false);
        playerUI.SetActive(true);
        spawn.SetActive(true);
        healthBars.SetActive(true);
        cameraBounds.SetActive(true);
    }

    public void nextButton(){
        if(line < script.Length){
            StopCoroutine(coroutine);
            coroutine = speak(script[line++]);
            StartCoroutine(coroutine);
        } else {
            skip();
        }
    }

    IEnumerator BounceArrows(){
        float t = 0;

        while(true){
            Vector2 tempL = leftArrowPos, tempU = upArrowPos;
            tempL.x += 50 * Mathf.Sin(t);
            tempU.y -= 50 * Mathf.Sin(t);

            leftArrowTrans.anchoredPosition = tempL;
            upArrowTrans.anchoredPosition = tempU;

            t += .25f;

            yield return null;
        }
    }
}
