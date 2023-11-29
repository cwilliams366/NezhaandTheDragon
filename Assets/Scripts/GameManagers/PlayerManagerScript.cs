using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerManagerScriptObject", menuName = "Scripts/ScriptableObjects/PlayerManagerScript", order = 1)]
public class PlayerManagerScript : ScriptableObject
{
    public GameStateScript GameStartState { get; set; }
    public GameObject CurrentPlayer { get; private set; }

    [Header("Spawn Data")]
    public string spawnGameTag;
    public GameObject playerPrefab;

    private void OnEnable()
    {
        LevelEventsScript.levelLoaded += SpawnthePlayer;
    }

    public void SpawnthePlayer(Transform defaultSpawnPoint)
    {
        if(GameStartState.PlayerSpawnPoint != "")
        {
            GameObject [] allAvalableSpawnPoints = GameObject.FindGameObjectsWithTag(spawnGameTag);
            bool spawnConfirmed = false;

            foreach (GameObject spawnPoints in allAvalableSpawnPoints)
            {
                if(GameStartState.PlayerSpawnPoint == spawnPoints.name)
                {
                    spawnConfirmed = true;
                    
                    CurrentPlayer = Instantiate(playerPrefab, spawnPoints.transform.position, Quaternion.identity);
                    break;
                }
            }
            if (!spawnConfirmed)
            {
                throw new MissingReferenceException("No Spot Found");
            }
        }
        else
        {
            CurrentPlayer = Instantiate(playerPrefab, defaultSpawnPoint.position, Quaternion.identity);
            Debug.Log("Player spawned at default location");
        }

        if (CurrentPlayer)
        {
            PlayerEventsScript.PlayerSpawned.Invoke(CurrentPlayer.transform);
        } 
        else
        {
            throw new MissingReferenceException("No player is currently active! Failed to spawn");
        }
    }

    private void OnDisable()
    {
        LevelEventsScript.levelLoaded -= SpawnthePlayer;
    }
}
