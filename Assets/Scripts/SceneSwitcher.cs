using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneSwitcher
{

    public enum Scene
    {
        GameScene,
    }
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
