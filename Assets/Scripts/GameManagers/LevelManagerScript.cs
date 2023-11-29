using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelManagerScriptObject", menuName = "Scripts/ScriptableObjects/LevelManagerScript", order = 1)]
public class LevelManagerScript : ScriptableObject
{
    public GameStateScript GameStartState { get; set; }

    private void OnEnable()
    {
        LevelEventsScript.levelExit += OnClosingTheLevel;
    }

    private void OnClosingTheLevel(SceneAsset nextLevel, string newPlayerSpawnPoint)
    {
        GameStartState.PlayerSpawnPoint = newPlayerSpawnPoint;
        SceneManager.LoadScene(nextLevel.name, LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        LevelEventsScript.levelExit -= OnClosingTheLevel;
    }

}
