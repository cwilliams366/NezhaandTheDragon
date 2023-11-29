using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    private GameObject[] spawnPoints;
    public GameObject [] enemyList;
    public static SpawnEnemies S;

    // Start is called before the first frame update
    private void Awake()
    {
        S = this;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SpawnAllEnemies()
    {
        if(spawnPoints.Length > 0)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Instantiate(enemyList[i], spawnPoints[i].transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("No enemy spawn points are present within current level");
        }
    }
}
