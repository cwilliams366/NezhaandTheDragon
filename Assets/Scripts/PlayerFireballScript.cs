using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Damage the enemy
        EnemyScript.S.TakeDamage(1);
        //Destroy GameObject
        Destroy(this.gameObject);
    }
}
