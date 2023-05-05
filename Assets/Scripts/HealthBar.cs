using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public GameObject building;
    public float health;
    public float maxHealth = 2500;


    void Update()
    {
        health = building.GetComponent<boundary>().health; // would change to get the current health of the object
        healthBarImage.fillAmount = (health / maxHealth);

    }
}