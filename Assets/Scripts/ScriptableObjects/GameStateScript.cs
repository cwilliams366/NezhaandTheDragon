using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateScriptObject", menuName = "Scripts/ScriptableObjects/GameStateScript", order = 1)]
public class GameStateScript : ScriptableObject
{
    [Header("Spawn Information")]
    public string PlayerSpawnPoint = "";
}
