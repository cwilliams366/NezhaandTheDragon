
using UnityEditor;

using UnityEngine;
using UnityEngine.Events;

public class LevelEventsScript
{
    public static UnityAction<Transform> levelLoaded;

    public static UnityAction<SceneAsset, string> levelExit;
}
