using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void startGame()
    {
        int index = Random.Range(1, SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(index);
    }
}
