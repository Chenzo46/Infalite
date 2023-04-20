using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 direction;
    private float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed * Time.fixedDeltaTime * 100;
    }

    public void setDirection(Vector2 Dir)
    {
        direction = Dir;
    }

    public void setSpeed(float spd)
    {
        speed = spd;
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag.Equals("Player"))
        {
            gameObject.SetActive(false);
            PlayerController.instance.die();
        }
        else if (collision.tag.Equals("wall"))
        {
            gameObject.SetActive(false);
        }
        
    }
}
