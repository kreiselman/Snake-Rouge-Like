using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width = 5;
    private int[,] map;
    ArrayList valid_spots = new ArrayList();
    public int num_rooms = 3;
    public int num_food = 10;
    public GameObject wall;
    public GameObject floor;
    public GameObject food;
    Random random = new Random();

    private void Start(){
        map = new int[18 * width, 18 * width];
        int[] rooms = new int[num_rooms];

        for (int i = 0; i < num_rooms; i++)
        {
            int num = Random.Range(0, map.Length / (18 * 18));
            while (true)
            {
                bool isIn = false;
                foreach (int element in rooms)
                    if (element == num)
                        isIn = true;
                if (!isIn)
                    break;

                num = Random.Range(0, map.Length / (18 * 18));
            }
            rooms[i] = num;
            Debug.Log("Random Num: " + rooms[i]);
        }

        for (int i = 0; i < (18 * width); i += 18)
        {
            for (int j = 0; j < (18 * width); j += 18)
            {
                foreach (int element in rooms)
                {
                    if ((i / 18) == (element % width)
                        && (j / 18) == (element / width))
                        MakeRoom(i / 18, j / 18);
                    //Debug.Log("Element found at:" + element);
                }
            }
        }

        MakeHallways(rooms);

        string line = "";
        for (int i = 0; i < (18 * width); i++)
        {
            for (int j = 0; j < (18 * width); j++)
            {
                if (map[i, j] == 1)
                {
                    Instantiate(floor, new Vector3(i + 1.0f, j + 1.0f, 0), Quaternion.identity);
                    int[] cords = new int[2];
                    cords[0] = i;
                    cords[1] = j;
                    valid_spots.Add(cords);
                }
                else if (map[i, j] == 0)
                {
                    Instantiate(wall, new Vector3(i + 1.0f, j + 1.0f, 0), Quaternion.identity);
                }
                line += string.Format("{0}", map[i, j]);
            }
            line += System.Environment.NewLine;
        }
        Debug.Log(line);
        AddFood();
    }

    public void AddFood()
    {
        for(int i = 0; i < num_food; i++){
            int[] spot = (int[]) (valid_spots[Random.Range(0, valid_spots.Count)]);
            Instantiate(food, new Vector3(spot[0] + 1, spot[1] + 1, 0), Quaternion.identity);
        }
    }

    private void MakeRoom(int x, int y)
    {
        int roomWidth = Random.Range(14, 18);
        int roomHeight = Random.Range(14, 18);
        int mapX = x * 18;
        int mapY = y * 18;

        for (int i = 0; i < roomHeight - 0; i++)
        {
            for (int j = 0; j < roomWidth - 0; j++)
                map[j + mapX, i + mapY] = 1;
        }
    }


    private void MakeHallways(int[] rooms)
    {
        int before = rooms[rooms.Length - 1];
        int after = rooms[1];
        int current = rooms[0];
        for (int i = 1; i < rooms.Length - 1; i++)
        {
            PlaceHallway(before, after, current);
            before = rooms[i - 1];
            after = rooms[i + 1];
            current = rooms[i];
        }

        before = rooms[rooms.Length - 2];
        after = rooms[0];
        current = rooms[rooms.Length - 1];
        PlaceHallway(before, after, current);
    }

    public void PlaceHallway(int before, int after, int current)
    {
        int end = 0;
        int index = before;
        for(int p = 0; p < 2; p++){
            if ((index / width) * 18 < (current / width) * 18){
                for (int k = (index / width) * 18; k < 1 + (current / width) * 18; k++){
                    for (int j = 0; j < 2; j++){
                        map[j + ((current % width) * 18), k] = 1;
                    }
                }
            }
            else{
                for (int k = (current / width) * 18; k < 1 + (index / width) * 18; k++){
                    for (int j = 0; j < 2; j++){
                        map[j + ((current % width) * 18), k] = 1;
                    }
                }
            }
            end = (index / width) * 18;
            if ((index % width) * 18 < (current % width) * 18)
            {
                for (int k = ((index % width) * 18); k < 1 + (current % width) * 18; k++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        map[k, j + end] = 1;
                    }
                }
            }
            else
            {
                for (int k = (current % width) * 18; k < 1 + (index % width) * 18; k++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        map[k, j + end] = 1;
                    }
                }
            }
            index = after;
        }
    }
}
