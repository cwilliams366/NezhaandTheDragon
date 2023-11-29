using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Singleton
    public static Player S;
    //Public Variables
    public int health = 3;
    public int power = 3;
    public int totalHealth;
    public float iFrames;
    public bool HasDied = false;
    public Material mat;
    public SkinnedMeshRenderer ren;
    public Collider2D BC;
    private void Awake()
    {
        if (S)
        {
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
       // ren = GetComponent<Renderer>();
        totalHealth = health = 3; 
        power = 3;
        iFrames = 1f;
       // ren.material.color = Color.blue;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!HasDied)
        {
            BC.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyWeapon")
        {
            if (health > 1)
            {
                health--;
                GameManagerScript.S.PlayerTookDamage();
                //Initate Iframes
                StartCoroutine(RecoveryState());
                
            }
            else
            {
                //KillPlayer();
                HasDied = true;
                BC = GetComponent<CapsuleCollider2D>();
                BC.enabled = false;
                GameManagerScript.S.PlayerDied();
            }
        }
        else if(collision.gameObject.tag == "Wall" && MagicScript.S.IsGrappleModeActive)
        {
            MagicScript.S.LatchedOn();
        }
    }

    //Restore the player's stats
    public void RestoreStats()
    {
        health = 3;
        power = 3;
        StartCoroutine(RecoveryState());
    }
    private IEnumerator RecoveryState()
    {
        Color color = ren.material.color;
     
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for(int i = 0; i < 5; i++)
        {
            color = ren.material.color;
            color.a = 0f;
            ren.material.color = color;
            yield return new WaitForSeconds(iFrames / (5*2));
            color.a = 1;
            ren.material.color = color;
            yield return new WaitForSeconds(iFrames / (5 * 2));
        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    //Kill the player
    public void KillPlayer()
    {
        gameObject.SetActive(false) ;
    }
}
