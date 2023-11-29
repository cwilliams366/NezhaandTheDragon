using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventsScript : MonoBehaviour
{
    public static UnityAction<Transform> PlayerSpawned;

    public static UnityAction PlayerDespawned;
}
