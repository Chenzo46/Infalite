using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlatform : MonoBehaviour
{
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private float pivotRadius = 0.5f;
    [SerializeField] private LayerMask ball;
    [SerializeField] private GameObject platform;
    [SerializeField] private float dissapearTime = 1f;

    private bool hasDissapeared = true;

    void Update()
    {
        checkForBall();
    }

    private void checkForBall()
    {
        Collider[] hits = Physics.OverlapSphere(pivotPoint.position, pivotRadius, ball);

        if(hits.Length != 0 && hits[0].transform.parent == null)
        {
            StopCoroutine(dissapear());
            hasDissapeared = false;
            hits[0].transform.position = pivotPoint.position;
            platform.SetActive(true);
        }
        else if(!hasDissapeared)
        {
            StartCoroutine(dissapear());
        }
    }

    private IEnumerator dissapear()
    {
        yield return new WaitForSeconds(dissapearTime);
        hasDissapeared = true;
        platform.SetActive(false);
    }
}
