using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void StartGame()
    {
        // Load scene 0 (game scene)
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
