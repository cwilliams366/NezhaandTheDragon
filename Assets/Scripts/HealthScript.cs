using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    //Health Indicator
    public int health, numOfHeartsVisible;
    //UI Essentials
    public Image[] hearts;
    public Sprite fullHearts;
    public Sprite emptyHeart;
 
    private void Start()
    {
        GetHealthCount();
       
    }

    private void Update()
    {
        GetHealthCount();

        //If health amount goes beyond the max amount
        if(health > numOfHeartsVisible)
        {
            health = numOfHeartsVisible;
        }

        //Display the proper health amount on screen
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHearts; 
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if(i < numOfHeartsVisible)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void GetHealthCount()
    {
        this.health = Player.S.health;
        this.numOfHeartsVisible = Player.S.totalHealth;
    }
}
