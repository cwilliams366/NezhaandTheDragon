using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

    [Header("Item Name")]
    public GameObject magic;
    public GameObject health;
    public GameObject talisman;
    public GameObject spirit;
    public GameObject coin;
    public GameObject pearl;
    public Rigidbody2D rb;
    public Vector3 playerPos;
    private bool isMagnetized = false;
    private float magnetPol = 15f;


    void Update()
    {
      
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            
            if (this.tag == "Coin")
            {
                //Play respective sound
                SoundManagerScript.S.PlayRespectiveSound(coin.tag);

                //Add to current coin amount
                GameManagerScript.S.UpdateCoinAmount(); 

            }
            else if(this.tag == health.tag)
            {
                //Play respective sound
                SoundManagerScript.S.PlayRespectiveSound(health.tag);

                //Add to the current health amount
                Player.S.health += 1;

            }
            else if(this.tag == magic.tag)
            {
                //Play respective sound
                SoundManagerScript.S.PlayRespectiveSound(magic.tag);

                //Add to the current magic amount
                MagicScript.S.playerMagic += 0.15f;
            }
            else if (this.tag == talisman.tag)
            {
                //Play respective sound
                SoundManagerScript.S.PlayRespectiveSound(talisman.tag);

                //Add to current talisman amount
                GameManagerScript.S.UpdateTalismanAmount();
            }
            else if (this.tag == spirit.tag)
            {
                //Play respective sound
                SoundManagerScript.S.PlayRespectiveSound(spirit.tag);

                //Add to the current spirit amount
                GameManagerScript.S.UpdateSpiritManaAmount();

            }
            else if(this.tag == pearl.tag)
            {
                //Add to current pearl amount
                GameManagerScript.S.UpdatePearlAmount();
            }
            //Destroy the game object
            Destroy(this.gameObject);
        }
    }
}
