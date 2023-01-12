using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Scene scene = Scene.StartMenu;

    public void LoadScene()
    {
        GameSceneManager.LoadScene(scene);
    }
}
