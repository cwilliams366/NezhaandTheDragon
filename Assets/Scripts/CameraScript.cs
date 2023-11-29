using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float cameraFollowSpeed;
    public float yOffset;
    public static Transform target;
    private Vector3 updatedPosition;
    // Start is called before the first frame update
    void Start()
    {
        cameraFollowSpeed = 3.0f;
        yOffset = 2.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (target)
        {
            //Update the current position of the camera every frame based on player's position
            updatedPosition = new Vector3(target.position.x, target.position.y + yOffset, -10.0f);
            transform.position = Vector3.Slerp(transform.position, updatedPosition, cameraFollowSpeed * Time.deltaTime);
        }
    }

    public void ReadjustCamera(Transform newPosition)
    {
        updatedPosition.x = newPosition.position.x;
    }
}
