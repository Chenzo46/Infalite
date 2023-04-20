using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeRadius : MonoBehaviour
{
    [SerializeField] private float Radius;
    [SerializeField] private float force;
    [SerializeField] private LayerMask all;

    public bool applied = true;

    private void Update()
    {
        //RaycastHit hit;
        if (gameObject.activeInHierarchy && applied)
        {
            foreach (Collider c in hits())
            {
                if(c.gameObject.GetComponent<Rigidbody>() != null)
                {
                    
                    //Vector2 dir = c.transform.position - transform.position;
                    //c.gameObject.GetComponent<Rigidbody>().velocity = Vector2.zero;
                    c.gameObject.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, Radius, 1f, ForceMode.Impulse);
                    applied = false;
                     
                }

            }
            
        }
    }

    private Collider[] hits()
    {
        return Physics.OverlapSphere(transform.position, Radius, all);
    }
}
