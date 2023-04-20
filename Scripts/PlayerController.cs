using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, PlayerMovement.IPlayerActions
{

    [Header("Movement Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravityM = 2;
    [SerializeField] private Transform cast1;
    [SerializeField] private Transform cast2;

    [Space(15)]

    [Header("Ball Variables")]
    [SerializeField] private float grabDistance;
    [SerializeField] private LayerMask ball;
    [SerializeField] private GameObject orb;
    [SerializeField] private Transform hold;
    [SerializeField] private float throwHeight, throwLength;
    [SerializeField] private float deathDist;
    [SerializeField] private GameObject fField;

    [Space(15)]

    [Header("Gun Properties")]
    [SerializeField] private float shootIntensity = 5f;
    [SerializeField] private Transform CrystalSpot;
    [SerializeField] private Transform shootSpot;
    [SerializeField] private CrystalFollow cr_OBJ;
    [SerializeField] private float gunReload = 2f;
    [SerializeField] private float shootTime = 0.1f;

    [Space(15)]

    [Header("Stats")]
    [SerializeField] private float currentEnergy = 10f;
    [SerializeField] private Slider energyBar;
    [SerializeField] private float currentHealth = 20f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float healthDecMultiplier = 1f;
    [SerializeField] private Slider specialSlider;
    [SerializeField] private float currentSpecial = 20f;

    //Components
    private PlayerMovement pm;
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer spr;

    //Vectors
    private Vector3 lookDir;

    //Booleans
    private bool isHolding = false;
    private bool reloading = false;
    private bool canShoot = true;
    private bool charged = false;
    private bool playedExit = false;
    private bool playedEnter = true;

    //Gamobjects
    private GameObject currentBullet;

    //Reference variables
    float reloadRef;
    float changeRef;
    float healthRef;
    float specialRef;

    public static PlayerController instance;


    private void OnEnable()
    {
        pm.Player.Enable();

    }

    private void OnDisable()
    {
        pm.Player.Disable();
    }

    void Awake()
    {
        instance = this;

        pm = new PlayerMovement();
        pm.Player.SetCallbacks(this);

        energyBar.maxValue = currentEnergy;
        healthSlider.maxValue = currentHealth;
        specialSlider.maxValue = currentSpecial;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();

        Physics.gravity = new Vector3(0, -9.807f * gravityM, 0);

        lookDir = Vector3.right;

        cr_OBJ.readyForShot(CrystalSpot, CrystalSpot.rotation);

        float scaleMultiplse = (deathDist / 0.6f)*2;

        fField.transform.localScale = new Vector3(scaleMultiplse, scaleMultiplse, scaleMultiplse);

        currentBullet = null;
    }


    void Update()
    {

        healthSlider.value = Mathf.SmoothDamp(healthSlider.value, currentHealth, ref healthRef, 0.05f);
        if (!reloading)
        {
            energyBar.value = Mathf.SmoothDamp(energyBar.value, currentEnergy, ref changeRef, 0.05f);
        }

        if (isAwayFromBall()) 
        {
            currentHealth -= healthDecMultiplier * Time.deltaTime;
        }
        else if(!isAwayFromBall() && currentHealth < healthSlider.maxValue)
        {
            currentHealth += healthDecMultiplier/2 * Time.deltaTime;
        }

        if(currentBullet != null)
        {
            currentBullet.transform.position = CrystalFollow.sharedInstance.shoot().position;
        }

        if(isAwayFromBall() && !playedExit)
        {
            playedExit = true;
            playedEnter = false;
            AudioManager.instance.playSound("Exit Portal");
        }
        else if (!isAwayFromBall() && !playedEnter)
        {
            playedEnter = true;
            playedExit = false;
            AudioManager.instance.playSound("Enter Portal");
        }


        specialSlider.value = Mathf.SmoothDamp(specialSlider.value, currentSpecial, ref specialRef, 0.05f);

        if (currentEnergy <= 0f && !reloading)
        {
            StartCoroutine(waitForReload());
        }

        if (lookDir == Vector3.right)
        {
            shootSpot.localPosition = new Vector2(1, 0);
            shootSpot.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (lookDir == Vector3.left)
        {
            shootSpot.localPosition = new Vector2(-1, 0);
            shootSpot.rotation = Quaternion.Euler(0, 0, -90);
        }

        anim.SetBool("jumping", !isGrounded());
        anim.SetBool("holding", isHolding);
        
    }
    private void FixedUpdate()
    {
        Vector2 mov = pm.Player.Move.ReadValue<Vector2>();

        rb.velocity = new Vector2(mov.x * speed * Time.fixedDeltaTime * 100, rb.velocity.y);

        anim.SetBool("walking", mov.x != 0);

        if (mov.x > 0)
        {
            lookDir = Vector3.right;
            spr.flipX = false;

        }
        else if (mov.x < 0)
        {
            lookDir = Vector3.left;
            spr.flipX = true;
        }
    }

    private bool isAwayFromBall()
    {
        
        return Vector2.Distance(transform.position, orb.transform.position) > deathDist;
    }

    private bool isGrounded()
    {
        return Physics.Raycast(cast1.position,Vector3.down ,checkRadius,groundMask) || Physics.Raycast(cast2.position, Vector3.down, checkRadius, groundMask);
    }

    private bool canGrab()
    {
        return Physics.Raycast(transform.position, lookDir,grabDistance, ball);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded() && context.performed)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            AudioManager.instance.playSound("Jump");
        }
        
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canGrab())
            {
                AudioManager.instance.playSound("pick up");
                orb.transform.position = hold.position;
                orb.GetComponent<Rigidbody>().isKinematic = true;
                orb.GetComponent<SphereCollider>().isTrigger = true;
                orb.transform.parent = transform;
                isHolding = true;
            }
            else if (isHolding)
            {
                AudioManager.instance.playSound("throw");
                orb.GetComponent<Rigidbody>().isKinematic = false;
                orb.GetComponent<SphereCollider>().isTrigger = false;
                orb.transform.parent = null;
                orb.GetComponent<Rigidbody>().AddForce((Vector3.up * throwHeight) + (lookDir * throwLength) + rb.velocity, ForceMode.Impulse);
                isHolding = false;
            }
        }
       
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //TODO (N/A)
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && !isHolding)
        {
            if (!reloading && canShoot)
            {
                //AudioManager.instance.playSound("charge one");

                    
                currentBullet = objectPool.SharedInstance.GetPooledObject("bullet");

                currentBullet.GetComponent<Rigidbody>().velocity = Vector2.zero;

                currentBullet.GetComponent<bulletBehaviour>().setPiercing(false);

                currentBullet.transform.position = cr_OBJ.shoot().position;
                currentBullet.SetActive(true);
                currentBullet.GetComponent<Animator>().SetTrigger("smallCharge");

                cr_OBJ.readyForShot(shootSpot, Quaternion.Euler(0, 0, 90 * -lookDir.x));
                StartCoroutine(chargeShot());
            }
        }
        else if (context.canceled && !isHolding)
        {
            if (!reloading && canShoot && currentBullet != null)
            {
                if (currentEnergy >= 0f && !charged)
                {
                    StopAllCoroutines();
                    currentBullet.GetComponent<Rigidbody>().AddForce(lookDir * shootIntensity, ForceMode.Impulse);
                    currentEnergy -= 2f;
                    currentBullet.GetComponent<Animator>().SetTrigger("sMove");

                }
                else if (currentEnergy >= 0f && charged)
                {
                    currentBullet.GetComponent<Rigidbody>().AddForce(lookDir * shootIntensity * 2, ForceMode.Impulse);
                    currentBullet.GetComponent<Animator>().SetTrigger("bMove");
                    currentEnergy -= 5f;
                }
                AudioManager.instance.playSound("shoot");
                currentBullet = null;
                StartCoroutine(waitForNextShot());
            }
        }
        
    }

    public void die()
    {
        gameObject.SetActive(false);
    }

    public void interruptReload()
    {
        StopCoroutine(waitForReload());
        reloading = false;
    }

    private IEnumerator waitForReload()
    {
        energyBar.value = currentEnergy;
        reloading = true;
        yield return new WaitForSeconds(gunReload);
        currentEnergy = energyBar.maxValue;
        reloading = false;
    }

    private IEnumerator chargeShot()
    {
        charged = false;
        //AudioManager.instance.playSound("Wind Up");
        yield return new WaitForSeconds(1.2f);
        AudioManager.instance.playSound("charge two");
        currentBullet.GetComponent<Animator>().SetTrigger("bigCharge");
        currentBullet.GetComponent<bulletBehaviour>().setPiercing(true);
        charged = true;
    }

    private IEnumerator waitForNextShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootTime);
        canShoot = true;
    }

    public void increaseMagic(float num)
    {
        currentEnergy += num;
    }

    public void increaseSpecial(float inc)
    {
        currentSpecial += inc;
    }
}
