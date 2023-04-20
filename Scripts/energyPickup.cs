using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyPickup : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private float energyInc;
    [SerializeField] private float magneticDistance;
    [SerializeField] private float pullSpeed = 3f;
    private SphereCollider col;
    private Collider[] verts;
    private Rigidbody rb;

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        verts = Physics.OverlapSphere(transform.position, col.radius, player);
        if (verts.Length != 0)
        {
            verts[0].GetComponent<PlayerController>().increaseMagic(energyInc);
            PlayerController.instance.interruptReload();
            gameObject.SetActive(false);
        }


    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < magneticDistance)
        {
            Vector2 Dir = PlayerController.instance.transform.position - transform.position;

            rb.MovePosition((Vector2)transform.position + (Dir * pullSpeed * Time.deltaTime));
        }
    }
}
