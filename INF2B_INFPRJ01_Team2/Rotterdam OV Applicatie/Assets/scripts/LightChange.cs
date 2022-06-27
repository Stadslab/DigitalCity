using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject sunlight;
    GameObject darkLight;
    void Start()
    {
        DateTime currentDateTime = DateTime.Now;
        Debug.Log(currentDateTime);
        sunlight = GameObject.Find("Light");
        darkLight = GameObject.Find("DarkLight");

        //GameObject.Find("DarkLight").transform.eulerAngles = new Vector3(180, -30, 0);
        if (currentDateTime.Hour > 5 && currentDateTime.Hour <= 12) 
        {
            sunlight.transform.eulerAngles = new Vector3(0, -30, 0);
            darkLight.transform.eulerAngles = new Vector3(180, -30, 0);
        }
        else if (currentDateTime.Hour > 12 && currentDateTime.Hour <= 15)
        {
            sunlight.transform.eulerAngles = new Vector3(75, -30, 0);
           darkLight.transform.eulerAngles = new Vector3(255, -30, 0);
        }
        else if (currentDateTime.Hour > 15 && currentDateTime.Hour <= 18)
        {
            sunlight.transform.eulerAngles = new Vector3(150, -30, 0);
            darkLight.transform.eulerAngles = new Vector3(330, -30, 0);
        }
        else if ((currentDateTime.Hour > 18 && currentDateTime.Hour <= 21) )
        {
            sunlight.transform.eulerAngles = new Vector3(225, -30, 0);
            darkLight.transform.eulerAngles = new Vector3(75, -30, 0);
        }
        else if((currentDateTime.Hour > 21 && currentDateTime.Hour <= 24) || (currentDateTime.Hour <= 5))
        {
           sunlight.transform.eulerAngles = new Vector3(300, -30, 0);
            darkLight.transform.eulerAngles = new Vector3(120, -30, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sunlight.transform.Rotate(1f * Time.deltaTime, 0, 0);
        darkLight.transform.Rotate(1f * Time.deltaTime, 0, 0);
        
    }
}
