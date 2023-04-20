using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    [SerializeField] private float dartSpeed = 5f;
    [SerializeField] private float shootSpeed = 2f;

    private bool canShoot = false;

    private void Awake()
    {
        StartCoroutine(reload());
    }

    void Update()
    {
        if (canShoot)
        {
            Dart dart = objectPool.SharedInstance.GetPooledObject("dart").GetComponent<Dart>();
            dart.setDirection(transform.right);
            dart.setSpeed(dartSpeed);
            dart.transform.position = transform.position + transform.right/2;
            dart.transform.rotation = transform.rotation;
            dart.gameObject.SetActive(true);
            StartCoroutine(reload());
        }
    }

    IEnumerator reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }
}
