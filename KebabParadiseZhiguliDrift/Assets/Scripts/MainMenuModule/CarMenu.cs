using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMenu : MonoBehaviour
{
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
