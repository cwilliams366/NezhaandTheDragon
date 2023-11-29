using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireballScript : MonoBehaviour
{
    public float speed = 7.0f;
    public Transform player;
    public GameObject EnemyType;
    Rigidbody2D rb;
    Vector2 moveDirection;


    private void Start()
    {
        //Find the player within game
        if (!player)
        {
            GameObject searchPlayer = GameObject.Find("Player");
            if (player)
            {
                player = searchPlayer.transform;
            }
        }
        if (!player)
        {
            return;
        }

        //Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();

        //Rotate bullet to face player's current location
        moveDirection = (player.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(this.gameObject, 6.0f);

    }



    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.name.Equals("Player") || Collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit");
            Destroy(this.gameObject);
        }
    }
}
