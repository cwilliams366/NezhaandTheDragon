using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneScript : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            //Player.S.KillPlayer();
            GameManagerScript.S.PlayerDied();
        }
        if(collision.gameObject.tag == "Enemy")
        {
            EnemyScript.S.KillEnemy();
        }
    }

}
