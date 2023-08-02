using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainTime;

 
    void Update()
    {
        if(remainTime>0)
        {
            remainTime -= Time.deltaTime;
        }
        else if (remainTime <0)
        {
            remainTime = 0;

            timerText.color = Color.red;
        }
        int minutes = Mathf.FloorToInt(remainTime / 60);
        int seconds = Mathf.FloorToInt(remainTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        
    }
}
