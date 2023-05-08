using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class materialTracker : MonoBehaviour
{
    private int max = 999999;

    public TextMeshProUGUI[] main;
    public TextMeshProUGUI[] blacksmith;
    public TextMeshProUGUI[] alchemist;
    
    public static int XPCount = 0;
    public static int WoodCount = 0;
    public static int StoneCount = 0;
    public static int IronCount = 0;

    public void Update()
    {
        main[0].text = XPCount.ToString();
        main[1].text = StoneCount.ToString();
        main[2].text = WoodCount.ToString();
        main[3].text = IronCount.ToString();

        blacksmith[0].text = StoneCount.ToString();
        blacksmith[1].text = WoodCount.ToString();
        blacksmith[2].text = IronCount.ToString();
        blacksmith[3].text = StoneCount.ToString();
        blacksmith[4].text = WoodCount.ToString();
        blacksmith[5].text = IronCount.ToString();
        blacksmith[6].text = StoneCount.ToString();
        blacksmith[7].text = WoodCount.ToString();
        blacksmith[8].text = IronCount.ToString();

        alchemist[0].text = XPCount.ToString();
        alchemist[1].text = XPCount.ToString();
        alchemist[2].text = XPCount.ToString();
    }

    public void changeXP(int amount){
        if(XPCount > 0 || XPCount < max) XPCount += amount;
    }

    public void changeWood(int amount){
        if(WoodCount > 0 || WoodCount < max) WoodCount += amount;
    }

    public void changeStone(int amount){
        if(StoneCount > 0 || StoneCount < max) StoneCount += amount;
    }

    public void changeIron(int amount){
        if(IronCount > 0 || IronCount < max) IronCount += amount;
    }
}
