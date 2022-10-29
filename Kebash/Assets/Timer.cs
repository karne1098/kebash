using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeValue = 10; //90 seconds
    public TextMeshProUGUI timeTextTMP;

    // Update is called once per frame
    void Update()
    {
        if(timeValue > 0) 
        {
            timeValue -= Time.deltaTime;
        }
        else //no time left
        {
            timeValue = 0; //locks to 0
        }     

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            Debug.Log("done");
            timeToDisplay = 0;   
        }

        //calculating minutes and seconds values
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeTextTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
