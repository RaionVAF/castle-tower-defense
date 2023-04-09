using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    private bool time = true;
    private Light lite;
    public Color night = new Color(0.3843137f, 0.5176471f, 0.6705883f, 1f);
    public Color day = new Color(1f, 0.9568627f, 0.8392157f, 1f);
    private float r, g, b;

    void Start()
    {
        r = (day.r - night.r)/10f;
        g = (day.g - night.g)/10f;
        b = (day.b - night.b)/10f;

        lite = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine(Change());
        }
    }

    IEnumerator Change()
    {
        if(time)
        {
            for(float i = 1; i <= 10; i += 1f){
                lite.color = new Color((lite.color.r - r), (lite.color.g - g), (lite.color.b - b), 1);
                yield return null;
            }

            time = false;
        } else
        {
            for(float i = 1; i <= 10; i += 1f){
                lite.color = new Color((lite.color.r + r), (lite.color.g + g), (lite.color.b + b), 1);
                yield return null;
            }

            time = true;            
        }
    }
}
