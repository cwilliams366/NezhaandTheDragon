using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxScript : MonoBehaviour
{
    private float length, distance, startingPosition;
    public GameObject mainCamera;
    public float ParallaxMagic;

    // Start is called before the first frame update
   private void Start()
    {
        startingPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float temp = (mainCamera.transform.position.x * (1 - ParallaxMagic));
        distance = (mainCamera.transform.position.x *  ParallaxMagic);
        transform.position = new Vector3(startingPosition + distance, transform.position.y, transform.position.z);
        if(temp > (startingPosition + length))
        {
            startingPosition += length;
        }
        else if(temp < (startingPosition - length))
        {
            startingPosition -= length;
        }
    }
}
