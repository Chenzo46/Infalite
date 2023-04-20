using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Method 1")]
    [SerializeField] private float y_Offset = 5;

    [SerializeField] private Vector2 y_Clamp;
    [SerializeField] private Vector2 x_Clamp;

    [Range(0.001f, 1f)]
    [SerializeField] private float smoothIntensity = 0.01f;

    [Header("Method 2")]
    [SerializeField] private Vector3 offset = new Vector3(0,0,-5);

    Vector3 reference;



    void FixedUpdate() => lookAtTargetClamped();


    private void lookAtTargetClamped()
    {

        /* Camera Follow Method 1
        float y_Rotation = (Mathf.Atan2(target.position.x - transform.position.x, target.position.z - transform.position.z)) * Mathf.Rad2Deg;
        float x_Rotation = (Mathf.Atan2(target.position.z - transform.position.z, target.position.y - transform.position.y)) * Mathf.Rad2Deg - 90f;

        y_Rotation = Mathf.Clamp(y_Rotation, y_Clamp.x, y_Clamp.y);
        x_Rotation = Mathf.Clamp(x_Rotation, x_Clamp.x, x_Clamp.y);

        transform.position = new Vector3(transform.position.x, y_Offset, transform.position.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(x_Rotation, y_Rotation, 0), smoothIntensity);

        */

        //Camera Follow Method 2

        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref reference, smoothIntensity);

    }

}
