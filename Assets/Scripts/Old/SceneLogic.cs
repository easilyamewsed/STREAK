using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLogic : MonoBehaviour
{
    public List<string> sceneNames;
    string randomScene;
    
    void Start()
    {
        sceneNames.Add("Evan's Level");
        sceneNames.Add("Alex's Level");
        sceneNames.Add("Gabby's Level");

        string currentScene = SceneManager.GetActiveScene().name;
        
        do
        {
            randomScene = sceneNames[Random.Range(0, sceneNames.Count)];
        
        } while (currentScene == randomScene);

        
    }
    
    public void SelectRandomLevel()
    {
        SceneManager.LoadScene(randomScene);
    }
}