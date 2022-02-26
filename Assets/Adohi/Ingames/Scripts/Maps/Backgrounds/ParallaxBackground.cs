using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float moveInterval;

    private Vector3 previousCameraPosition;

    public Camera camera;

    public float moveRatio;

    public void Update()
    {
        moveInterval = camera.transform.position.x - previousCameraPosition.x;
        transform.position += Vector3.right * moveInterval * moveRatio;
        previousCameraPosition = camera.transform.position;
    }


}
