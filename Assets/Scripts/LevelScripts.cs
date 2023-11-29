using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class LevelScripts : MonoBehaviour
{
    public Transform [] platforms;
    private GameObject [] gObj;
    
    //Prep the levels
    private void Awake()
    {
        gObj = GameObject.FindGameObjectsWithTag("Ground");
        platforms = new Transform[gObj[0].transform.parent.childCount];
        for(int i = 0; i < gObj.Length; i++)
        {
            //gObj[i].SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < platforms.Length;i++)
        {
            platforms[i] = gObj[i].transform;
            
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(GameManagerScript.S.currentState == GameState.Playing)
        {
            for(int i = 0; i < gObj.Length;i++)
            {
                gObj[i].SetActive(true);
            }
        }
    }
}
