using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{

    public Button StartButton;

    public Slider RoomSlider;

    public Slider WidthSlider;

    public Slider FoodSlider;

    public Text RoomText;

    public Text WidthText;

    public Text FoodText;

    private void Awake()
    {
        Button btn = StartButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        RoomSlider.value = SceneSwitcher.MapRooms;

        WidthSlider.value = SceneSwitcher.MapWidth;

        FoodSlider.value = SceneSwitcher.MapFood;
        

        WidthSlider.maxValue = 10;
    }



    void Update()
    {
        RoomSlider.maxValue = WidthSlider.value * WidthSlider.value;
        RoomText.text = "Number of Rooms: " + RoomSlider.value;
        WidthText.text = "Width of Game Grid: " + WidthSlider.value;
        FoodText.text = "Number of Starting Food: " + FoodSlider.value;
    }



    void TaskOnClick()
    {
        SceneSwitcher.MapFood = (int)FoodSlider.value;
        SceneSwitcher.MapRooms = (int)RoomSlider.value;
        SceneSwitcher.MapWidth = (int)WidthSlider.value;
        SceneSwitcher.Load(SceneSwitcher.Scene.GameScene);
    }
}
