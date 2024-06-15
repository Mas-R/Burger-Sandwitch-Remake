using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int maxObject;
    [SerializeField] private ObjectDetector[] detectors;
    public bool draggingStarted = false;
    Scene currentScene;
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        detectors = FindObjectsOfType<ObjectDetector>();
        maxObject = detectors.Length;
        foreach(ObjectDetector detector in detectors) { 
            detector.levelManager = this;
        }
    }

    public void CheckObject(List<GameObject> go)
    {
        Debug.Log("Start calculate");
        if(go.Count == maxObject && go[0].GetComponent<ObjectDetector>().bread == ObjectDetector.Bread.Bottom && go[go.Count-1].GetComponent<ObjectDetector>().bread == ObjectDetector.Bread.Top)
        {
            PlayerPrefs.SetInt("Level", currentScene.buildIndex+1);
            FindObjectOfType<NextLevelPanel>().gameObject.transform.GetChild(0).gameObject.SetActive(true);

        }
    }
}
