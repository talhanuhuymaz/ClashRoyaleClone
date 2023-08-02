using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class TimerCount : MonoBehaviour
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
 
        }
        int minutes = Mathf.FloorToInt(remainTime / 60);
        int seconds = Mathf.FloorToInt(remainTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        
    }
}
