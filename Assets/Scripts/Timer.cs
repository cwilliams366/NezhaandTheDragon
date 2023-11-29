using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Timer Essentials")]
    public float time;
    private float currentTime;
    private float minutes;
    private float seconds;
    private float miliSeconds;
    private bool isPaused = false;
    public TMP_Text TimerText;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if(GameManagerScript.S.currentState == GameState.Playing)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                time = 0;
                GameManagerScript.S.GameOver();
            }

            //Show the remaining time;
            ShowRemainingTime(time);
        }
        else if(GameManagerScript.S.currentState == GameState.Paused)
        {
            currentTime = time;
            ShowRemainingTime(currentTime);
            //isPaused = true;
        }
           
    }

    private void ShowRemainingTime(float displayTime)
    {
        if(displayTime < 0)
        {
            displayTime = 0f;
        }

        //Calculate minutes
        float minutes = Mathf.FloorToInt(displayTime / 60);
        //Calculate seconds
       float  seconds = Mathf.FloorToInt(displayTime % 60);
        //Calculate miliseconds
       float  miliSeconds =   displayTime % 1 * 1000;
        //Diaply Current Time
        TimerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, miliSeconds);
    }
}
