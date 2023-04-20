using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private int spawnSpeed = 2;
    [SerializeField] private bool tiedToRoomEvent = false;

    [SerializeField] private int maxEnemiesToSpawn = 5;

    private int spawnedEnemies = 0;

    private bool attackStarted = false;

    private bool canSpawn = false;

    private void OnEnable()
    {
        if (tiedToRoomEvent)
        {
            enemyDefeat.OnAttackStarted += toggleStart;
            enemyDefeat.OnEnemiesDefeated += toggleStart;
        }
    }

    private void Update()
    {

        if (tiedToRoomEvent && attackStarted && canSpawn && spawnedEnemies < maxEnemiesToSpawn)
        {
            spawn();
        }
        else if (spawnedEnemies < maxEnemiesToSpawn && !tiedToRoomEvent && canSpawn)
        {
            spawn();
        }
    }

    void toggleStart(int a)
    {
        attackStarted = !attackStarted;
        canSpawn = !canSpawn;
        maxEnemiesToSpawn = a;
        StopCoroutine(spawnEnemy());
    }

    private IEnumerator spawnEnemy()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnSpeed);
        canSpawn = true;
        
    }

    private void spawn()
    {
        //Debug.Log("enemy spawned", gameObject);
        GameObject g = objectPool.SharedInstance.GetPooledObject("Enemy");
        g.transform.position = transform.position;
        g.GetComponent<EnemyBehaviour>().resetHealth();
        g.SetActive(true);
        spawnedEnemies++;
        StartCoroutine(spawnEnemy());
    }


}
