using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneSwitcher
{

    public enum Scene
    {
        GameScene,
        MainMenu,
    }
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static int MapWidth = 5;

    public static int MapRooms = 4;

    public static int MapFood = 10;

}
