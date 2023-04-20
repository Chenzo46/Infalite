using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    [SerializeField] private LevelManager lvlManage;

    private void OnEnable()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            endLevel();
        }
    }

    private void endLevel()
    {
        lvlManage.incLevelsBeat();
        lvlManage.setPreviousLevel();
        lvlManage.toNextLevel();
    }
}
