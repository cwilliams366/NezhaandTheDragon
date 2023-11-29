using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropScript : MonoBehaviour
{
    [Header("Item Specifications")]
    public float dropForce;
    private Rigidbody2D rbItem;


    // Start is called before the first frame update
    private void Start()
    {
        //Initialize values
        dropForce = 4.0f;
        rbItem = GetComponent<Rigidbody2D>();
        rbItem.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse); 
    }

    // Update is called once per frame
   private void Update()
    {
        
    }
}
