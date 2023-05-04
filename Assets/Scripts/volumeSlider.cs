using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //to access UI features

public class volumeSlider : MonoBehaviour
{

    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void changeVol()
    {
        AudioListener.volume = slider.value;
    }

}
