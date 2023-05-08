using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Upgrades2 : MonoBehaviour
{
    // THIS UPGRADE SCRIPT IS SIDE SECTOR SPECIFIC

    // Get Towers
    public Ballista leftTower, rightTower, centerTower,
                    leftCannon, rightCannon, centerCannon;
    // Get Buttons
    public Button leftStoneButton, leftWoodButton, leftIronButton,
                  rightStoneButton, rightWoodButton, rightIronButton,
                  centerStoneButton, centerWoodButton, centerIronButton;
    // Get Button Texts
    public TMPro.TMP_Text leftStoneText, leftWoodText, leftIronText,
                          rightStoneText, rightWoodText, rightIronText,
                          centerStoneText, centerWoodText, centerIronText;
    // All tier and tower specific bools
    bool leftStoneTier1, leftStoneTier2, leftStoneTier3, leftStoneTier4,
         leftWoodTier1, leftWoodTier2, leftWoodTier3, leftWoodTier4,
         leftIronTier,
         rightStoneTier1, rightStoneTier2, rightStoneTier3, rightStoneTier4,
         rightWoodTier1, rightWoodTier2, rightWoodTier3, rightWoodTier4,
         rightIronTier,
         centerStoneTier1, centerStoneTier2, centerStoneTier3, centerStoneTier4,
         centerWoodTier1, centerWoodTier2, centerWoodTier3, centerWoodTier4,
         centerIronTier;

    // Damage upgrades are a flat 50 increase
    public void increaseLeftTowerDamage()
    {
        if (leftStoneTier1 == false && materialTracker.StoneCount >= 6)
        {
            // Update tower damage
            leftTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 6;

            // Toggle bool to trigger next upgrade tier
            leftStoneTier1 = true;
            // Edit button text for next tier
            leftStoneText.text = "9";
        }
        else if (leftStoneTier1 == true && leftStoneTier2 == false && materialTracker.StoneCount >= 9)
        {
            // Update tower damage
            leftTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 9;

            // Toggle bool to trigger next upgrade tier
            leftStoneTier2 = true;
            // Edit button text for next tier
            leftStoneText.text = "12";
        }
        else if (leftStoneTier2 == true && leftStoneTier3 == false && materialTracker.StoneCount >= 12)
        {
            // Update tower damage
            leftTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 12;

            // Toggle bool to trigger next upgrade tier
            leftStoneTier3 = true;
            // Edit button text for next tier
            leftStoneText.text = "15";
        }
        else if (leftStoneTier3 == true && leftStoneTier4 == false && materialTracker.StoneCount >= 15)
        {
            // Update tower damage
            leftTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 15;

            // Toggle bool to trigger next upgrade tier
            leftStoneTier4 = true;
            // Remove button since this is the last tier
            leftStoneButton.gameObject.SetActive(false);
        }
    }

    public void increaseRightTowerDamage()
    {
        if (rightStoneTier1 == false && materialTracker.StoneCount >= 6)
        {
            // Update tower damage
            rightTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 6;

            // Toggle bool to trigger next upgrade tier
            rightStoneTier1 = true;
            // Edit button text for next tier
            rightStoneText.text = "9";
        }
        else if (rightStoneTier1 == true && rightStoneTier2 == false && materialTracker.StoneCount >= 9)
        {
            // Update tower damage
            rightTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 9;

            // Toggle bool to trigger next upgrade tier
            rightStoneTier2 = true;
            // Edit button text for next tier
            rightStoneText.text = "12";
        }
        else if (rightStoneTier2 == true && rightStoneTier3 == false && materialTracker.StoneCount >= 12)
        {
            // Update tower damage
            rightTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 12;

            // Toggle bool to trigger next upgrade tier
            rightStoneTier3 = true;
            // Edit button text for next tier
            rightStoneText.text = "15";
        }
        else if (rightStoneTier3 == true && rightStoneTier4 == false && materialTracker.StoneCount >= 15)
        {
            // Update tower damage
            rightTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 15;

            // Toggle bool to trigger next upgrade tier
            rightStoneTier4 = true;
            // Remove button since this is the last tier
            rightStoneButton.gameObject.SetActive(false);
        }
    }

    public void increaseCenterTowerDamage()
    {
        if (centerStoneTier1 == false && materialTracker.StoneCount >= 6)
        {
            // Update tower damage
            centerTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 6;

            // Toggle bool to trigger next upgrade tier
            centerStoneTier1 = true;
            // Edit button text for next tier
            centerStoneText.text = "9";
        }
        else if (centerStoneTier1 == true && centerStoneTier2 == false && materialTracker.StoneCount >= 9)
        {
            // Update tower damage
            centerTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 9;

            // Toggle bool to trigger next upgrade tier
            centerStoneTier2 = true;
            // Edit button text for next tier
            centerStoneText.text = "12";
        }
        else if (centerStoneTier2 == true && centerStoneTier3 == false && materialTracker.StoneCount >= 12)
        {
            // Update tower damage
            centerTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 12;

            // Toggle bool to trigger next upgrade tier
            centerStoneTier3 = true;
            // Edit button text for next tier
            centerStoneText.text = "15";
        }
        else if (centerStoneTier3 == true && centerStoneTier4 == false && materialTracker.StoneCount >= 15)
        {
            // Update tower damage
            centerTower.damageOutput += 50;
            // Decrease resource values
            materialTracker.StoneCount -= 15;

            // Toggle bool to trigger next upgrade tier
            centerStoneTier4 = true;
            // Remove button since this is the last tier
            centerStoneButton.gameObject.SetActive(false);
        }
    }

    // Flat 1% increase
    public void increaseLeftTowerRate()
    {
        if (leftWoodTier1 == false && materialTracker.WoodCount >= 12)
        {
            // Update tower damage
            leftTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 12;

            // Toggle bool to trigger next upgrade tier
            leftWoodTier1 = true;
            // Edit button text for next tier
            leftWoodText.text = "20";
        }
        else if (leftWoodTier1 == true && leftWoodTier2 == false && materialTracker.WoodCount >= 20)
        {
            // Update tower damage
            leftTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 20;

            // Toggle bool to trigger next upgrade tier
            leftWoodTier2 = true;
            // Edit button text for next tier
            leftWoodText.text = "34";
        }
        else if (leftWoodTier2 == true && leftWoodTier3 == false && materialTracker.WoodCount >= 34)
        {
            // Update tower damage
            leftTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 34;

            // Toggle bool to trigger next upgrade tier
            leftWoodTier3 = true;
            // Edit button text for next tier
            leftWoodText.text = "45";
        }
        else if (leftWoodTier3 == true && leftWoodTier4 == false && materialTracker.WoodCount >= 45)
        {
            // Update tower damage
            leftTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 45;

            // Toggle bool to trigger next upgrade tier
            leftWoodTier4 = true;
            // Remove button since this is the last tier
            leftWoodButton.gameObject.SetActive(false);
        }
    }

    public void increaseRightTowerRate()
    {
        if (rightWoodTier1 == false && materialTracker.WoodCount >= 12)
        {
            // Update tower damage
            rightTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 12;

            // Toggle bool to trigger next upgrade tier
            rightWoodTier1 = true;
            // Edit button text for next tier
            rightWoodText.text = "20";
        }
        else if (rightWoodTier1 == true && rightWoodTier2 == false && materialTracker.WoodCount >= 20)
        {
            // Update tower damage
            rightTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 20;

            // Toggle bool to trigger next upgrade tier
            rightWoodTier2 = true;
            // Edit button text for next tier
            rightWoodText.text = "34";
        }
        else if (rightWoodTier2 == true && rightWoodTier3 == false && materialTracker.WoodCount >= 34)
        {
            // Update tower damage
            rightTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 34;

            // Toggle bool to trigger next upgrade tier
            rightWoodTier3 = true;
            // Edit button text for next tier
            rightWoodText.text = "45";
        }
        else if (rightWoodTier3 == true && rightWoodTier4 == false && materialTracker.WoodCount >= 45)
        {
            // Update tower damage
            rightTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 45;

            // Toggle bool to trigger next upgrade tier
            rightWoodTier4 = true;
            // Remove button since this is the last tier
            rightWoodButton.gameObject.SetActive(false);
        }
    }

    public void increaseCenterTowerRate()
    {
        if (centerWoodTier1 == false && materialTracker.WoodCount >= 12)
        {
            // Update tower damage
            centerTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 12;

            // Toggle bool to trigger next upgrade tier
            centerWoodTier1 = true;
            // Edit button text for next tier
            centerWoodText.text = "20";
        }
        else if (centerWoodTier1 == true && centerWoodTier2 == false && materialTracker.WoodCount >= 20)
        {
            // Update tower damage
            centerTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 20;

            // Toggle bool to trigger next upgrade tier
            centerWoodTier2 = true;
            // Edit button text for next tier
            centerWoodText.text = "34";
        }
        else if (centerWoodTier2 == true && centerWoodTier3 == false && materialTracker.WoodCount >= 34)
        {
            // Update tower damage
            centerTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 34;

            // Toggle bool to trigger next upgrade tier
            centerWoodTier3 = true;
            // Edit button text for next tier
            centerWoodText.text = "45";
        }
        else if (centerWoodTier3 == true && centerWoodTier4 == false && materialTracker.WoodCount >= 45)
        {
            // Update tower damage
            centerTower.shootingRate -= 0.25f;
            // Decrease resource values
            materialTracker.WoodCount -= 45;

            // Toggle bool to trigger next upgrade tier
            centerWoodTier4 = true;
            // Remove button since this is the last tier
            centerWoodButton.gameObject.SetActive(false);
        }
    }

    // Upgrade tower to cannon (damage and rate conversions are handled in Ballista.cs)
    public void upgradeLeftTowerToCannon()
    {
        if (leftIronTier == false && materialTracker.IronCount >= 10)
        {
            // Set ballista inactive and cannon active
            leftTower.gameObject.SetActive(false);
            leftCannon.gameObject.SetActive(true);
            // Set new left tower member to cannon and set damage
            leftTower = leftCannon;

            // Decrease resource values
            materialTracker.IronCount -= 10;

            // Toggle bool
            leftIronTier = true;
            // Remove button since this is the last tier
            leftIronButton.gameObject.SetActive(false);
        }
    }

    public void upgradeRightTowerToCannon()
    {
        if (rightIronTier == false && materialTracker.IronCount >= 10)
        {
            // Set ballista inactive and cannon active
            rightTower.gameObject.SetActive(false);
            rightCannon.gameObject.SetActive(true);
            // Set new right tower member to cannon
            rightTower = rightCannon;

            // Decrease resource values
            materialTracker.IronCount -= 10;

            // Toggle bool
            rightIronTier = true;
            // Remove button since this is the last tier
            rightIronButton.gameObject.SetActive(false);
        }
    }

    public void upgradeCenterTowerToCannon()
    {
        if (centerIronTier == false && materialTracker.IronCount >= 10)
        {
            // Set ballista inactive and cannon active
            centerTower.gameObject.SetActive(false);
            centerCannon.gameObject.SetActive(true);
            // Set new center tower member to cannon
            centerTower = centerCannon;

            // Decrease resource values
            materialTracker.IronCount -= 10;

            // Toggle bool
            centerIronTier = true;
            // Remove button since this is the last tier
            centerIronButton.gameObject.SetActive(false);
        }
    }
}   
