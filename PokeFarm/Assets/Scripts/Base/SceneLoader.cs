using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Scene scene = Scene.StartMenu;

    public void LoadScene()
    {
        SceneManager.LoadScene((int) scene);
        Debug.Log($"Scene [{scene}] was loaded.");
    }
}
