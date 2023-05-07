using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class boundary : MonoBehaviour
{
    public bool isAttackable = true;
    public float health;
    public float maxHealth;
    public Slider slider;
    public GameObject healthBar;

    public GameObject alchemist, blacksmith;
    private bool isAlchemistMenuOpen, isBlacksmithMenuOpen;

    private NavMeshObstacle obstacle;

    void Start(){
        slider.maxValue = maxHealth;
        health = maxHealth;
        slider.value = health;
        obstacle = GetComponent<NavMeshObstacle>();
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
            healthBar.SetActive(false);
            // obstacle.carving = false;
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
