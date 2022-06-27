using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSwitch : MonoBehaviour
{
    public Camera MainCam;
    public Camera Cam1;
    public Camera Cam2;
    public Camera Cam3;
    public Camera Cam4;

    TimeSystem timeSystem;

    // Start is called before the first frame update
    void Start()
    {
        //getting the text UI
        timeSystem = FindObjectOfType<TimeSystem>();

        MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Cam1 = GameObject.Find("Camera1").GetComponent<Camera>();
        Cam2 = GameObject.Find("Camera2").GetComponent<Camera>();
        Cam3 = GameObject.Find("Camera3").GetComponent<Camera>();
        Cam4 = GameObject.Find("Camera4").GetComponent<Camera>();

        timeSystem.TimeText.color = Color.white;
        MainCam.enabled = true;
        Cam1.enabled = false;
        Cam2.enabled = false;
        Cam3.enabled = false;
        Cam4.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleDropDown(int val)
    {
        if (val == 0)
        {
            timeSystem.TimeText.color = Color.white;
            MainCam.enabled = true;
            Cam1.enabled = false;
            Cam2.enabled = false;
            Cam3.enabled = false;
            Cam4.enabled = false;
        }
        if (val == 1)
        {
            timeSystem.TimeText.color = Color.black;
            MainCam.enabled = false;
            Cam1.enabled = true;
            Cam2.enabled = false;
            Cam3.enabled = false;
            Cam4.enabled = false;
        }
        if (val == 2)
        {
            timeSystem.TimeText.color = Color.black;
            MainCam.enabled = false;
            Cam1.enabled = false;
            Cam2.enabled = true;
            Cam3.enabled = false;
            Cam4.enabled = false;
        }
        if (val == 3)
        {
            timeSystem.TimeText.color = Color.black;
            MainCam.enabled = false;
            Cam1.enabled = false;
            Cam2.enabled = false;
            Cam3.enabled = true;
            Cam4.enabled = false;
        }
        if (val == 4)
        {
            timeSystem.TimeText.color = Color.black;
            MainCam.enabled = false;
            Cam1.enabled = false;
            Cam2.enabled = false;
            Cam3.enabled = false;
            Cam4.enabled = true;
        }
    }
}
