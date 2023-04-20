using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    private bool isPiercing = false;

    private Animator anim;

    private void Awake() => anim = GetComponent<Animator>();

    private void OnTriggerEnter(Collider other)
    {
        if(!other.tag.Equals("Player") && !other.tag.Equals("bullet") &&! other.tag.Equals("energy pickup") && !other.tag.Equals("ignore"))
        {
            
            GameObject explode = objectPool.SharedInstance.GetPooledObject("explosion");
            

            if (other.tag.Equals("Enemy") && !isPiercing)
            {
                Vector2 dir = other.transform.position - transform.position;
                other.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(-dir);
                anim.SetTrigger("restart");
                objectPool.SharedInstance.givePooledObject(gameObject);
                AudioManager.instance.playSound("pierce");

            }
            else if (other.tag.Equals("Enemy") && isPiercing)
            {
                Vector2 dir = other.transform.position - transform.position;
                other.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(dir);
                AudioManager.instance.playSound("pierce");
            }
            else
            {
                anim.SetTrigger("restart");
                objectPool.SharedInstance.givePooledObject(gameObject);
                AudioManager.instance.playSound("Wall Explode");
            }

            explode.transform.position = gameObject.transform.position;
            explode.SetActive(true);
        }


    }

    public void setPiercing(bool p)
    {
        isPiercing = p;
    }

}
