using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.UI;
using SpriteGlow;

public class HUDManagerScript : MonoBehaviour
{
    [Header("HUD Values")]
    public Transform LivesSet;
    public Transform PearlSet;
    public Transform HealthSet;
    public GameObject litHealthImage;
    public GameObject dullHealthImage;
    public GameObject pearlImage;
    public GameObject newPearlImage;
    public GameObject lifeImage;


    private Stack<GameObject> lives = new Stack<GameObject>();
    private Stack<GameObject> pearls = new Stack<GameObject>();
    private Stack<GameObject> health = new Stack<GameObject>();
    private static HUDManagerScript HUD;
    

    public static HUDManagerScript HUDSetter
    {
        get
        {
            if (!HUD)
            {
                HUD = FindObjectOfType<HUDManagerScript>();
            }
            return HUD;
        }
    }

    public void SetAvailableHealth(int count)
    {
        for(int i = 0; i < count; i++)
        {
            health.Push(Instantiate(litHealthImage, HealthSet));
        }
    }

    public void GetAvailablePearls(int count)
    {
        Debug.Log(PearlSet.childCount);
        for(int i = count-1; i >= 0; i--)
        {
            pearls.Push(Instantiate(pearlImage, PearlSet));
        }
    }

    public void IncrementLives(int totalLives)
    {
        for(int i = 0; i < totalLives; i++)
        {
            lives.Push(Instantiate(lifeImage, LivesSet));
        }

    }

    public void RemoveLife()
    {
        Destroy(lives.Pop());
    }

    public void ChangePearl(int index)
    {
      
        Destroy(pearls.Pop());
        Instantiate(newPearlImage, PearlSet);
    }

    public void ChangeLitHealthToDull()
    {
        Destroy(health.Pop());
        Instantiate(dullHealthImage, HealthSet);
    }
}
