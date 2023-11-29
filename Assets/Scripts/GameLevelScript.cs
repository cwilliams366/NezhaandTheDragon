using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelScript : MonoBehaviour

{
    [Header("Spawn Point Info")]
    public Transform defaultSpawnPoint;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log(defaultSpawnPoint);
        LevelEventsScript.levelLoaded.Invoke(defaultSpawnPoint);
    }



}
