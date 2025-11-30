using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using LogitechG29.Sample.Input;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private InputControllerReader _inputControllerReader;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _volume;

    public void Play()
    {
        //StartCoroutine(LoadYourAsyncScene());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
    }

    public void SaveSettings()
    {
        try
        {
            string json = JsonUtility.ToJson(_slider.value);
            Debug.Log(json);
            StreamWriter writer = null;
            #if UNITY_EDITOR
            writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "volume.json");
            #else
            writer = new StreamWriter(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "volume.json");
            #endif
            writer.Write(json);
        }
        catch (Exception e)
        {

        }
        
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");

        // Не активировать сцену сразу после загрузки
        asyncLoad.allowSceneActivation = false;

        // Ждем пока сцена загрузится
        while (!asyncLoad.isDone)
        {
            // Прогресс загрузки от 0 до 0.9, а затем ждем активации
            if (asyncLoad.progress >= 0.9f)
            {

            }

            
            yield return null;
        }
        
        asyncLoad.allowSceneActivation = true;
        yield return null;
    }

    public void OnSliderValueChanged()
    {
        _volume.text = $"Громкость: {(int) (_slider.value * 100)}";
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _inputControllerReader.Steering));
    }
    
    
}
