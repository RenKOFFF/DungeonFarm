using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum Scene
{
    StartMenu = 0,
    MainScene = 1,
    Autobattle = 4,
}

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    public static void LoadScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public static void LoadScene(Scene scene)
    {
        LoadScene((int) scene);
    }

    private void Awake()
    {
        Instance = this;
    }
}
