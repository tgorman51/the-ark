using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneListSO", menuName = "Scriptable Objects/SceneListSO")]
public class SceneListSO : ScriptableObject
{
    public List<String> sceneNames;
    
    public int _currentIndex = 0;

    public void LoadNext()
    {
        if (_currentIndex < sceneNames.Count - 1)
        {
            SceneManager.LoadScene(sceneNames[++_currentIndex]);
        }
        else
        {
            ResetIndex();
            SceneManager.LoadScene(sceneNames[_currentIndex]);
        }
    }

    public void ResetIndex()
    {
        _currentIndex = 0;
    }
}
