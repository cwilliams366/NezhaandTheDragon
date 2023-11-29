using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimSightScript : MonoBehaviour
{
    //Singleton
    public static AimSightScript S;
 
    private Vector3 aimPoint;
    private Camera cam;
    public GameObject aimPointer;

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
        //Get the main camera
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //Set the object to be invisible
        aimPointer = GameObject.FindGameObjectWithTag("AimPoint");
        aimPointer.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        //Update based on mouse position
        AimPointer();
    }

    private void AimPointer()
    {
        //Adjust the aim sight point
        aimPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rot = aimPoint - transform.position;
        float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
