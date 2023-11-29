using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameScript : MonoBehaviour
{
    
        // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && (GameManagerScript.S.currentState == GameState.Playing || GameManagerScript.S.currentState == GameState.Paused))
        {
            if(Time.timeScale == 1)
            {
                GameManagerScript.S.currentState = GameState.Paused;
                Time.timeScale = 0;
            }
            else
            {
                GameManagerScript.S.currentState = GameState.Playing;
                Time.timeScale = 1;
            }
        }
    }
}
