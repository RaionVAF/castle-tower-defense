using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class blacksmithUI : MonoBehaviour
{
    private int[] towerCCount;
    private int[] towerLCount;
    private int[] towerRCount;

    public TextMeshProUGUI[] towerC;
    public TextMeshProUGUI[] towerL;
    public TextMeshProUGUI[] towerR;
    // Start is called before the first frame update
    void Start()
    {
        towerCCount = new int[] {0,0,0};
        towerLCount = new int[] {0,0,0};
        towerRCount = new int[] {0,0,0};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void press(){

    }
}
