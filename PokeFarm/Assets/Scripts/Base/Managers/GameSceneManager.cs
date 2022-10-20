using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum Scene
{
    StartMenu = 0,
    MainScene = 1,
}

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public void LoadScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    private void Awake()
    {
        Instance = this;
    }
}
