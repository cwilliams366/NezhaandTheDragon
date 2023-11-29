using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLooksAtPlayer : MonoBehaviour
{
    public Transform player;
    Vector3 direction;
      // Update is called once per frame
    void Update()
    {
        if(!player)
        {
            GameObject searchPlayer = GameObject.Find("Player");
            if (player)
            {
                player = searchPlayer.transform;
            }
        }
        if(!player)
        {
            return;
        }

        direction = player.position - transform.position;
        direction.Normalize();

        float newAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion playersCurrentRotation  = Quaternion.Euler(0, 0, newAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playersCurrentRotation, 90.0f * Time.deltaTime);

    }
}
