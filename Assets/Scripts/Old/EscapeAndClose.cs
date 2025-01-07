using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToPreviousScene : MonoBehaviour
{
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If in scene 0, close the game
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                // Load the previous scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}