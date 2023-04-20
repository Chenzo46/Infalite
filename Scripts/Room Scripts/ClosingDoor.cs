using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoor : MonoBehaviour
{
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;

    private void OnEnable()
    {
        enemyDefeat.OnAttackStarted += closeDoor;
        enemyDefeat.OnEnemiesDefeated += openDoor;
    }


    public void closeDoor(int x)
    {
        transform.position = closePos.position;
    }

    public void openDoor(int x)
    {
        transform.position = openPos.position;
    }
}
