using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Level Manager")]
public class LevelManager : ScriptableObject
{
    public static LevelManager instance;

    [SerializeField] private int mainMenuSceneBuildIndex;
    private int previousLevel = -1;
    private int nextLevel;
    private int currentScene;
    private int levelsBeat;

    List<int> filteredScenes = new List<int>();


    private void Awake() => instance = this;
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += setScenes;
    }

    public void setScenes(Scene s, LoadSceneMode l)
    {
        //Debug.Log("Scenes Set");
        if(SceneManager.GetActiveScene().buildIndex != mainMenuSceneBuildIndex) 
        {
            currentScene = SceneManager.GetActiveScene().buildIndex;

            if (previousLevel == -1)
            {
                bool newSceneFound = false;

                while (!newSceneFound)
                {
                    
                    int randScene = Random.Range(1, SceneManager.sceneCountInBuildSettings);
                    if (currentScene == randScene || mainMenuSceneBuildIndex == randScene)
                    {
                        continue;
                    }
                    else
                    {
                        newSceneFound = true;
                        nextLevel = randScene;

                    }
                }
            }
            else
            {
                bool newSceneFound = false;

                while (!newSceneFound)
                {
                    int randScene = Random.Range(1, SceneManager.sceneCountInBuildSettings);
                    if (currentScene == randScene || previousLevel == randScene || mainMenuSceneBuildIndex == randScene)
                    {
                        continue;
                    }
                    else
                    {
                        newSceneFound = true;
                        nextLevel = randScene;
                    }
                }
            }
        }
    }

    public void setPreviousLevel()
    {
        //Debug.Log("Previous scene set");
        previousLevel = currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void incLevelsBeat()
    {
        //Debug.Log("level Beat Inc");
        levelsBeat++;
    }

    public int getLevelsBeat()
    {
        return levelsBeat;
    }

    public void toNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
    /*
    private List<Scene> getScenes()
    {
        List<Scene> ret = new List<Scene>();
        int length = SceneManager.sceneCountInBuildSettings;
        Debug.Log(length);

        for (int index = 0; index < length; index++)
        {
            ret.Add(SceneManager.GetSceneAt(index));
        }

        return ret;
    }
    */
}
