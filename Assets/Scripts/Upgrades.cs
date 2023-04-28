using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public void increaseDamage(int damageChange = 1)
    {
        gameObject.GetComponent<towerController>().damageOutput += damageChange;
    }

    public void increaseRange(int rangeChange = 1)
    {
        gameObject.GetComponent<SphereCollider>().radius += rangeChange;
    }

    public void increaseRate(int rateChange = 1)
    {
        gameObject.GetComponent<towerController>().shootingRate += rateChange;
    }
}
