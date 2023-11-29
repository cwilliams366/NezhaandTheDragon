using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using SpriteGlow;
using UnityEngine;

public class PlayerRespawnScript : MonoBehaviour
{
    //Singleton
    public static PlayerRespawnScript S;
    [Header("Checkpoint Data Values")]
    public SpriteGlowEffect sGlow;
    public Sprite sprite;
    private Transform currentCheckpoint;
    private Player playerStats;
    // Start is called before the first frame update
    private void Awake()
    {
        //Adjust player stats
        playerStats = GetComponent<Player>();

        if (S)
        {
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    public void Respawn()
    {
        //Set the location of the current checkpoint
        transform.position = currentCheckpoint.position;
        //Restore player's stats
        playerStats.RestoreStats();
        //Get the sprite component
        sprite = GetComponent<Sprite>();
        //Get SpriteGlowEffect component
        sGlow = GetComponent<SpriteGlowEffect>();
        //Adjust the camera to appropriate position
        Camera.main.GetComponent<CameraScript>().ReadjustCamera(currentCheckpoint.parent);
    }

    //Activate the checkpoint by initiate color change
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            //Set the current checkpoint and then deactivate it for a one time only use
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            //Sprite Color Arrangement
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            collision.gameObject.GetComponent<SpriteGlowEffect>().OutlineWidth = 3;
            Physics2D.IgnoreLayerCollision(7, 10, true);

        }
    }
}
