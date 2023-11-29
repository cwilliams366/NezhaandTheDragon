using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {Neutral, Stunned };
public class EnemyScript : MonoBehaviour
{


    [Header("Enemy Attributes")]
    public bool targetLock = false;
    public Transform Player;
    public float speed, power;
    public int damage, health;
    public EnemyState currentState = EnemyState.Neutral;
    public GameObject[] itemArr;
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D bc;
    public bool isMagenitized = false;
    private float magentPol = 10f;
    private bool isFacingRight = false;
    private Vector3 playerPos;
    public DetectionZone chargeZone;
    //Singleton
    public static EnemyScript S;
    private void Awake()
    {
        S = this;

        //rb = GetComponent<Rigidbody2D>();
       // animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        //health = 0;
        speed = 3f;
        //damage = 1;
        //power = 1.5f;

    }


    // Update is called once per frame
    private void Update()
    {
        //if (isMagenitized)
        //{
        //    Vector2 playerDirection = (playerPos - transform.position).normalized;
        //    rb.velocity = new Vector2(playerDirection.x, playerDirection.y) * magentPol;
        //}

        //Check for value change
        TargetLocked = chargeZone.DetectionList.Count > 0;

        //If player is detected, move towards the player
        if (targetLock)
        {
            //Vector2 playerDirection = (Player.position - transform.position).normalized;
            if (transform.position.x > Player.position.x)
            {
                if (isFacingRight)
                {
                    Vector3 tempScale = transform.localScale;
                    tempScale.x *= -(1.0f);
                    transform.localScale = tempScale;
                }

                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            else if(transform.position.x < Player.position.x)
            {
                if(!isFacingRight)
                {
                    Vector3 tempScale = transform.localScale;
                    tempScale.x *= -(1.0f);
                    transform.localScale = tempScale;
                }

                transform.position += Vector3.right * speed * Time.deltaTime;
            }
         }
    }

    public void beganMagnetism(Vector3 position)
    {
        playerPos = position;
        isMagenitized = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(currentState != EnemyState.Stunned)
            {
                //Set the amount of hitstun
                PlayerController.S.hitStunTimer = 0.5f;
                //Set the amount of knockback
                PlayerController.S.knockback = power * 4;
                //Enemy has made a hit confirm on Player
                PlayerController.S.enemyHitConfirm = true;
                //Check to see which direction was the collision
                if (collision.transform.position.x <= transform.position.x)
                {
                    PlayerController.S.rightDirectionKnockback = true;
                }
                else if (collision.transform.position.x > transform.position.x)
                {
                    PlayerController.S.rightDirectionKnockback = false;
                }
            }
        }
    }

    //Check if player is detected
    public bool TargetLocked
    {
        get { return targetLock; }

        private set
        {
            targetLock = value;
            animator.SetBool("PlayerDetected", value);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            //Drop an item?
            DropItem();
            //Play Death Animation
            animator.SetBool("IsDead", true);
            rb.gravityScale = 4; 
            bc.enabled = false;
            StartCoroutine(KillEnemy());
        }
        else
        {
            health -= damageAmount;
        }
     }

    public IEnumerator KillEnemy()
    {
        
        yield return new WaitForSeconds(0.5f);
        //Destroy Object
        Destroy(this.gameObject);
    }

    private void DropItem()
    {
        int rand = Random.Range(0, 2);

        if(rand == 0)
        {
            rand = Random.Range(0, 3);
            Instantiate(itemArr[rand], transform.position, Quaternion.identity);

        }
        else
        {
            Debug.Log("Sucks to be you!");
        }
    }

  
}
