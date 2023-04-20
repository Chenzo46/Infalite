using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask player;
    [SerializeField] private float radius;
    [SerializeField] private float health = 2;
    [SerializeField] private float knockBack = 3f;
    [SerializeField] private float KBTime = 0.25f;

    [SerializeField] private int energyPickups = 3;

    public delegate void EnemyAction();
    public static event EnemyAction onEnemyDeath;

    //[SerializeField] private bool gizmosOn;

    private float maxHealth = 0;

    private float c_Radius;

    private Rigidbody rb;
    private Transform plr;
    private SpriteRenderer spr;
    private Animator anim;

    private BoxCollider col;

    private bool hurtTime = false;

    private bool hit = false;

    private void Awake()
    {
        maxHealth = health;
        c_Radius = radius;
        rb = GetComponent<Rigidbody>();
        plr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
    }
    private void FixedUpdate()
    {
        if (isTouchingPlayer())
        {
            plr.gameObject.GetComponent<PlayerController>().die();
        }
        if (!hurtTime)
        {
            if (playerIsNear() || hit)
            {
                if (c_Radius != radius * 2)
                {
                    c_Radius *= 2;
                }

                rb.velocity = new Vector2(getPlayerDirection().x * speed * Time.fixedDeltaTime * 100, rb.velocity.y);
                //rb.MovePosition((Vector2)transform.position + getPlayerDirection() * speed * Time.deltaTime);
                anim.SetBool("walking", true);
            }
            else
            {
                c_Radius = radius;
                anim.SetBool("walking", false);
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
        }
        

       
    }

    private bool playerIsNear()
    {
        return Physics.CheckSphere(transform.position, c_Radius, player);
    }

    Vector2 prevDir;
    private Vector2 getPlayerDirection()
    {
        if(plr.transform.position.x - 0.1 < transform.position.x)
        {
            spr.flipX = true;
            prevDir = Vector2.left;
            return Vector2.left;
        }
        else if  (plr.transform.position.x + 0.1 > transform.position.x)
        {
            spr.flipX = false;
            prevDir = Vector2.right;
            return Vector2.right;
        }
        return prevDir;
    }

    private bool isTouchingPlayer()
    {
        return Physics.OverlapBox(transform.position, col.bounds.size/2, Quaternion.Euler(0,0,0), player).Length != 0;
    }

    public void TakeDamage()
    {
        anim.SetTrigger("hurt");

        health -= 1;

        hit = true;

        if(health == 0)
        {
            Die();
        }
    }

    public void TakeDamage(Vector2 Dir)
    {
        anim.SetTrigger("hurt");

        health -= 1;

        hit = true;

        if (health == 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(hurt(Dir));
        }

        
    }

    private void Die()
    {
        if (enemyDefeat.instance != null)
        {
            onEnemyDeath.Invoke();
        }
       
        for(int i = 0; i < energyPickups; i++)
        {
            GameObject g = objectPool.SharedInstance.GetPooledObject("energy pickup");
            float x = Random.Range(-3,4);
            float y = Random.Range(-3, 4);
            g.transform.position = transform.position;
            g.SetActive(true);
            g.GetComponent<Rigidbody>().AddForce(new Vector2(x,y), ForceMode.Impulse);
        }

        gameObject.SetActive(false);
    }

    public void resetHealth()
    {
        health = maxHealth;
    }

    public void flashDone()
    {
        anim.SetTrigger("hDone");
    }

    private IEnumerator hurt(Vector2 Dir)
    {
        hurtTime = true;
        rb.velocity = new Vector3(0,rb.velocity.y,0);
        rb.AddForce(-Dir * knockBack, ForceMode.Impulse);
        yield return new WaitForSeconds(KBTime);
        hurtTime = false;
    }

    

}
