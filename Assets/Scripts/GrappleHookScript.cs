using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookScript : MonoBehaviour
{

    //Hookshot Essentials
    [Header("HookShot Data")]
    public float hookshotSpeed;
    public float hookedDistance;
    public float maxDistance;
    public float xPosOnPlayer = -24.02f;
    public float yPosOnPlayer = 1.810005f;
    public Rigidbody2D rb;
    private Vector2 startHookingPoint;
    private bool IsCurrentlyHooking = false;
    private bool IsEnemyHooked = false;
    private bool IsDestinationHooked = false;
    private LineRenderer lineRen;

    //GameObjects
    private GameObject player;
    private GameObject enemy;
    private GameObject destination;

    //Sprite
    public SpriteRenderer sprite;
    public Animator animator;

    private void Awake()
    {
        //Get reference to player
        player = GameObject.FindGameObjectWithTag("Player");

        //Initialize the line renderer
        lineRen = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        //Setting hookshot values
        hookshotSpeed = 1.0f;
        hookedDistance = 0f;
        maxDistance = 10f;
        startHookingPoint = new Vector2(player.transform.position.x+xPosOnPlayer, player.transform.position.y+yPosOnPlayer);
        //Initialize Rigibody
        rb = GetComponent<Rigidbody2D>();
        
       
    }

    // Update is called once per frame
    private void Update()
    {
        //Maintain player's current position
        GetPlayerPositon();

        //Set the initial hooking point
        lineRen.SetPosition(0, startHookingPoint);
        //
        lineRen.SetPosition(1, transform.position);

        if(Input.GetKeyDown(KeyCode.R) && MagicScript.S.HasMeter() && !IsCurrentlyHooking && !IsEnemyHooked && !IsDestinationHooked)
        {
            Debug.Log("Hook Activated");
            ActivateHookShot();
        }
        //Return the hookshot
        HookShotRetract();
        //Bring in Enemy
        ReelInEnemy();
    }

    private void ActivateHookShot()
    {
        IsCurrentlyHooking = true;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * hookshotSpeed);
    }

    private void HookShotRetract()
    {
        if (IsCurrentlyHooking)
        {
            hookedDistance = Vector2.Distance(transform.position, startHookingPoint);
            if(hookedDistance > maxDistance)
            {
                rb.isKinematic = true;
                transform.position = startHookingPoint;
                IsCurrentlyHooking = false;

            }
        }
    }

    private void GetPlayerPositon()
    {
        startHookingPoint = new Vector2(player.transform.position.x + xPosOnPlayer, player.transform.position.y);
    }

    private void ReelInEnemy()
    {
        if (IsEnemyHooked)
        {
            Vector2 reelingDestination = new Vector2(startHookingPoint.x, enemy.transform.position.y);
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, reelingDestination,maxDistance);
            IsEnemyHooked = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Hooked");
            IsEnemyHooked = false;
            enemy = collision.gameObject;
        }
        else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            Debug.Log("Platform and/or Wall is Latched");
        }
    }
}
