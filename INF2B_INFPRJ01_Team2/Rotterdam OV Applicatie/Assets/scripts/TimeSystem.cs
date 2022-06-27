using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI TimeText;

    void Start()
    {
       
        
        
    }

    // Update is called once per frame
    void Update()
    {
        var timeNow = System.DateTime.Now;
        TimeText.text = timeNow.ToShortTimeString();
        
    }
}
