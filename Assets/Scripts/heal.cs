using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heal : MonoBehaviour
{
    public GameObject House1, House2, Windmill, FarmersMarket, Campfire, Farm, House3, TowerL, TowerR, House4, House5, Wall;
    private boundary structureScript;  
    public void repair(GameObject structure){
        structureScript = structure.GetComponent<boundary>();
        if (structureScript.health <= 0 && materialTracker.XPCount >= 50){
            structure.SetActive(true);
            structureScript.health = 500;
            materialTracker.XPCount -= 50;
        } else if (structureScript.health < structureScript.maxHealth && materialTracker.XPCount >= 50){
            structureScript.health = Mathf.Clamp(structureScript.health + 500, 0, structureScript.maxHealth);
            materialTracker.XPCount -= 50;
        }
    }

}
