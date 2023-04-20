using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFollow : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float moveTime;
    [SerializeField] private Transform bulletStart;
    public static CrystalFollow sharedInstance;

    //Reference variables
    Vector2 reference;
    float refVelocity;

    // Update is called once per frame

    private void Awake() => sharedInstance = this;

    void Update() 
    { 
        if(target != null)
        {
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref reference, moveTime);
            transform.eulerAngles = new Vector3(0, 0, Mathf.SmoothDamp(transform.eulerAngles.z, target.eulerAngles.z, ref refVelocity, moveTime));
        }
       
    }

    public void readyForShot(Transform point, Quaternion rotate)
    {
        target = point;
    }

    public Transform shoot()
    {
        return bulletStart;
    }
}
