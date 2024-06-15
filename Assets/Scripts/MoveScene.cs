using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    Scene currentScene;
    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(currentScene.buildIndex+1);
    }
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ResetLevelSaved()
    {
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene(1);
    }
    public void StartLevel()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            if(PlayerPrefs.GetInt("Level") >= 5)
            {
                PlayerPrefs.SetInt("Level", 5);
            }
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));

        }
        else
        {
            SceneManager.LoadScene(1);

        }

    }
   
}
