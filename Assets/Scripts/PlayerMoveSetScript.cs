using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSetScript : MonoBehaviour
{

    [Header("Player Attack Data")]
    public Transform swordAttackPoint;
    public Transform kickAttackPoint;
    public Transform punchAttackPoint;
    public float attackRange;
    public int attackDamage;
    public float attackRecovery;

    public Animator animator;
    public LayerMask enemyLayer;

    private void Start()
    {
        attackRange = 0.5f;
        attackDamage = 1;
        attackRecovery = 0.7f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Check what attack the player has performed
        MoveList();
    }

    //Get the player's movelist
    private void MoveList()
    {
        //Attack#1
        if (Input.GetKeyDown(KeyCode.Z) && PlayerController.S.hasLanded())
        {
            //Animate the attack
            animator.SetTrigger("StandingSlash");
            animator.SetTrigger("CloseRangeAttack");
            attackDamage = 2;
            //Perform the respective melee properties
            MeleeAttack();

        }//Aerial Attack#1
        else if (Input.GetKeyDown(KeyCode.Z) && !PlayerController.S.hasLanded())
        {
            //Animate the attack
            animator.SetTrigger("CloseRangeAttack");
            
            //Perform the respective melee properties
            MeleeAttack();
        }

        //Attack#2
        if (Input.GetKeyDown(KeyCode.C) && PlayerController.S.hasLanded())
        {
            animator.SetTrigger("LongRangeAttack");

        }//Aerial Attack#2
        else if (Input.GetKeyDown(KeyCode.C) && !PlayerController.S.hasLanded())
        {

        }

        //Kick
        if (Input.GetKeyDown(KeyCode.C) && PlayerController.S.hasLanded())
        {

        }//Aerial Kick
        else if (Input.GetKeyDown(KeyCode.C) && !PlayerController.S.hasLanded())
        {

        }
    }

    //Start the attack
    private void MeleeAttack()
    {
        //What's in range?
        Collider2D[] enemyDetection = Physics2D.OverlapCircleAll(swordAttackPoint.position, attackRange, enemyLayer);
       
        //Collection of colliders
        foreach (Collider2D enemy in enemyDetection)
        {
            Debug.Log("Hit Confirm!");
            enemy.GetComponent<EnemyScript>().currentState = EnemyState.Stunned;
            enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
        }
    }


     void OnDrawGizmosSelected()
    {
        if (swordAttackPoint)
        {
            Gizmos.DrawWireSphere(swordAttackPoint.position, attackRange);

        }
    }
}
