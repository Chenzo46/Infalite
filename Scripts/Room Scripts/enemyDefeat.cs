using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDefeat : MonoBehaviour
{
    [SerializeField] private int enemiesToKill = 0;
    [SerializeField] private LayerMask playerMask;

    private int enemiesKilled = 0;

    public delegate void EnemyRoom(int baseNum);
    public static event EnemyRoom OnEnemiesDefeated;
    public static event EnemyRoom OnAttackStarted;
    private bool roomEntered = false;

    public static enemyDefeat instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EnemyBehaviour.onEnemyDeath += incDeaths;
    }

    void incDeaths()
    {
        enemiesKilled++;

        if (enemiesKilled == enemiesToKill)
        {
            OnEnemiesDefeated.Invoke(enemiesToKill);
            clearAllEventDelegates();
        }
    }
    private void clearAllEventDelegates()
    {
        System.Delegate[] a = OnAttackStarted.GetInvocationList();
        System.Delegate[] b = OnEnemiesDefeated.GetInvocationList();

       
        foreach(System.Delegate d in a)
        {
            OnAttackStarted -= (d as EnemyRoom);
        }
        foreach (System.Delegate d in b)
        {
            OnEnemiesDefeated -= (d as EnemyRoom);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !roomEntered)
        {
            roomEntered = true;
            OnAttackStarted.Invoke(enemiesToKill);
        }
    }

}
