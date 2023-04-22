using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class materialTracker : MonoBehaviour
{
    private int max = 999999;

    public TextMeshProUGUI mainWood;
    public TextMeshProUGUI bsWood;

    public TextMeshProUGUI mainStone;
    public TextMeshProUGUI bsStone;

    public TextMeshProUGUI mainIron;
    public TextMeshProUGUI bsIron;
    
    public int WoodCount = 0;
    public int StoneCount = 0;
    public int IronCount = 0;

    public void Update()
    {
        mainWood.text = WoodCount.ToString();
        bsWood.text = WoodCount.ToString();

        mainStone.text = StoneCount.ToString();
        bsStone.text = StoneCount.ToString();

        mainIron.text = IronCount.ToString();
        bsIron.text = IronCount.ToString();
    }
}
