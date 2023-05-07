using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class heal : MonoBehaviour
{
    private boundary structureScript;  

    public void repair(GameObject structure){
        structureScript = structure.GetComponent<boundary>();

        if (structureScript.health <= 0 && materialTracker.XPCount >= 500)
        {
            // obstacle.carving = true;
            structure.SetActive(true);
            structureScript.health = 500;
            materialTracker.XPCount -= 500;
        } else if (structureScript.health < structureScript.maxHealth && materialTracker.XPCount >= 500)
        {
            structureScript.health = Mathf.Clamp(structureScript.health + 500, 0, structureScript.maxHealth);
            materialTracker.XPCount -= 500;
        }
    }

}
