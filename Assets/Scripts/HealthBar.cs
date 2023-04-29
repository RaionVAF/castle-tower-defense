using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public int health, maxHealth;


    public void UpdateHealthBar()
    {
        //health = GameObject.Find("HealthThing").GetComponent<HealthTest>().health; // would change to get the current health of the object
        //maxHealth = GameObject.Find("HealthThing").GetComponent<HealthTest>().maxHealth; // would change to get the max health of the object
        float fHealth = health * 1f; // converts both to floats so they do float division rather than int division
        float fMax = maxHealth * 1f;
        healthBarImage.fillAmount = (fHealth / fMax);
        // call updateHealthBar whenever the health of an object is changed
        //Have the health bar image type = filled, with fill method = Horizontal, and Fill origin = left
    }
}