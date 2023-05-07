using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boundary : MonoBehaviour
{
    public bool isAttackable = true;
    public float health;
    public float maxHealth;
    public Slider slider;
    public GameObject healthBar;

    public GameObject alchemist, blacksmith;
    private bool isAlchemistMenuOpen, isBlacksmithMenuOpen;

    void Start(){
        slider.maxValue = maxHealth;
        health = maxHealth;
        slider.value = health;
    }
   
    void Update()
    {
        // update upgrade menu bools
        isAlchemistMenuOpen = alchemist.GetComponent<alchemistController>().isUpgradeMenuOpen;
        isBlacksmithMenuOpen = blacksmith.GetComponent<blacksmithController>().isUpgradeMenuOpen;

        if (isAlchemistMenuOpen || isBlacksmithMenuOpen)
        {
            healthBar.SetActive(false);
        }

        slider.value = health;

        if (health <= 0){
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "playableKnight")
        {   
            healthBar.SetActive(true);
        } else {
            GameObject enemy = other.gameObject;
            if (enemy.tag == "enemyWeapon")
            {
                health -= enemy.GetComponent<Projectile>().damageOutput;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "playableKnight"){
            healthBar.SetActive(false);
        }
    }

    public void hit(float damage){
        health -= damage;
    }
}
