using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundary : MonoBehaviour
{
    public bool isAttackable = true;
    public float health = 2500;
   
    void Update()
    {
        if (health <= 0){
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject enemy = other.gameObject;
        if (enemy.tag == "enemyWeapon")
        {
            health -= enemy.GetComponent<Projectile>().damageOutput;
        }
    }

    public void hit(float damage){
        health -= damage;
    }
}
