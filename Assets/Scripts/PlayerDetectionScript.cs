using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionScript : MonoBehaviour
{
    public float detectionDistance, timeBetweenPointTravel;
    private bool PlayerDetected = false;
    private Vector2 direction;
    private string PlayerTag;
    private Transform  currentDestination;
    public Transform Player,Point1, Point2;
      // Start is called before the first frame update
    void Start()
    {
        PlayerTag = "Player";
        detectionDistance = 3.0f;
        timeBetweenPointTravel = 0.9f;
        currentDestination = Point1;
    }

    // Update is called once per frame
    void Update()
    {
     if(PlayerDetected && Player && EnemyScript.S.currentState == EnemyState.Neutral && EnemyScript.S.currentState != EnemyState.Stunned)
        {
            if(transform.position.x > Player.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * EnemyScript.S.speed * Time.deltaTime;

            }
            else if(transform.position.x < Player.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * EnemyScript.S.speed * Time.deltaTime;
            }
        }
     else
        {
            if (Player && EnemyScript.S.currentState == EnemyState.Neutral && EnemyScript.S.currentState != EnemyState.Stunned)
            {
                if (Vector2.Distance(transform.position, Player.position) <= detectionDistance)
                {
                    PlayerDetected = true;
                }

                if (currentDestination == Point2)
                {
                    StartCoroutine(Traverse());
                    //direction = (currentDestination.position - transform.position).normalized * EnemyScript.S.speed;
                    //rb.velocity = new Vector2(direction.x, 0);
                    if (Vector2.Distance(transform.position, currentDestination.position) <= 0.2f)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        currentDestination = Point1;
                    }
                }
                else
                {
                    StartCoroutine(Traverse());
                    //direction = (currentDestination.position - transform.position).normalized * EnemyScript.S.speed;
                    //rb.velocity = new Vector2(direction.x,0);
                    if (Vector2.Distance(transform.position, currentDestination.position) <= 0.2f)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        currentDestination = Point2;
                    }
                }
            }
        }
    }
    

    private IEnumerator Traverse()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentDestination.position,  EnemyScript.S.speed * Time.deltaTime);
        yield return new WaitForSeconds(timeBetweenPointTravel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(PlayerTag))
        {
            PlayerDetected = true;
            Player = collision.gameObject.transform;
        }
    }

 

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(PlayerTag))
        {
            PlayerDetected = false;
            Player = null;
        }
    }
}
