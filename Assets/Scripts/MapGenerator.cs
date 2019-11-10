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
    private Color[] colors = { new Color(146, 255, 138), new Color(138, 202, 255), new Color(255, 168, 138), new Color(172, 138, 255), new Color(237, 255, 138), new Color(255, 149, 138), new Color(255, 138, 187), new Color(236, 138, 255) };
    private int current_color = 0;

    private void Start()
    {
        map = new int[18 * width, 18 * width];
        int[] rooms = new int[num_rooms];
        for (int i = 0; i < rooms.Length; i++)
            rooms[i] = -1;

            for (int i = 0; i < num_rooms; i++)
        {
            int num = Random.Range(0, (width * width));
            while (true)
            {
                bool isIn = false;
                foreach (int element in rooms)
                {
                    if (element == num)
                        isIn = true;
                }
                if (!isIn)
                    break;

                num = Random.Range(0, (width*width));
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
                    if ((i / 18) == (element % width) && (j / 18) == (element / width))
                        MakeRoom(i / 18, j / 18);
                    //Debug.Log("Element found at:" + element);
                }
            }
        }
        GameObject border = Instantiate(wall, new Vector3(((18*width)/2)-.5f, -1, 0), Quaternion.identity);
        border.transform.localScale = new Vector3((18 * width)+2, 1);
        border = Instantiate(wall, new Vector3(((18 * width) / 2) - .5f, (18 * width), 0), Quaternion.identity);
        border.transform.localScale = new Vector3((18 * width) + 2, 1);
        border = Instantiate(wall, new Vector3(-1, ((18 * width) / 2) - .5f, 0), Quaternion.identity);
        border.transform.localScale = new Vector3(1, 18 * width);  
        border = Instantiate(wall, new Vector3((18 * width), ((18 * width) / 2) - .5f, 0), Quaternion.identity);
        border.transform.localScale = new Vector3(1, 18 * width);  

        MakeHallways(rooms);

        string line = "";
        for (int i = 0; i < (18 * width); i++)
        {
            GameObject placeWall;
            for (int j = 0; j < (18 * width); j++)
            {
                if(map[j, i] != 0)
                {
                    int[] cords = new int[2];
                    cords[0] = j;
                    cords[1] = i;
                    valid_spots.Add(cords);
                    if (j < (18 * width) && map[j + 1, i] == 0)
                        placeWall = Instantiate(wall, new Vector3(j + 1, i, 0), Quaternion.identity);
                    if (j > 0 && map[j - 1, i] == 0)
                        placeWall = Instantiate(wall, new Vector3(j - 1, i, 0), Quaternion.identity);
                    if (i < (18 * width) && map[j, i + 1] == 0)
                        placeWall = Instantiate(wall, new Vector3(j, i + 1, 0), Quaternion.identity);
                    if (i > 0 && map[j, i - 1] == 0)
                        placeWall = Instantiate(wall, new Vector3(j , i - 1, 0), Quaternion.identity);
                }
                line += string.Format("{0}", map[j, i]);
            }
            line += System.Environment.NewLine;
        }
        Debug.Log(line);
        for (int i = 0; i < num_food; i++)
            AddFood();
    }

    public void AddFood()
    {
            int[] spot = (int[])(valid_spots[Random.Range(0, valid_spots.Count)]);
            Instantiate(food, new Vector3(spot[0], spot[1], 0), Quaternion.identity);
    }

    public int[] getValid()
    {
        return (int[])(valid_spots[Random.Range(0, valid_spots.Count)]);
    }

    private void MakeRoom(int x, int y)
    {
        int roomWidth = Random.Range(13, 17);
        while(roomWidth % 2 == 0)
            roomWidth = Random.Range(13, 17);
        int roomHeight = Random.Range(13, 17);
        while(roomHeight % 2 == 0)
            roomHeight = Random.Range(13, 17);
        int mapX = x * 18;
        int mapY = y * 18;

        GameObject floor_inst = Instantiate(floor, new Vector3(mapX + roomWidth/2, mapY + roomHeight/2, 0), Quaternion.identity);
        floor_inst.GetComponent<SpriteRenderer>().color = new Color(colors[current_color].g / 255, colors[current_color].r / 255, colors[current_color].b / 255);
        floor_inst.transform.localScale = new Vector3(roomWidth, roomHeight);
        for(int i = 0; i < roomHeight;i++)
        {
            for(int j = 0; j < roomWidth; j++)
            {
                map[j + mapX, i + mapY] = 1;
            }
        }
        current_color++;
        if (current_color >= colors.Length) current_color = 0;
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
        for (int p = 0; p < 2; p++)
        {
            GameObject hallway;
            if ((index / width) * 18 < (current / width) * 18)
            {
                //hallway = Instantiate(floor, new Vector3(j + ((current % width) * 18), k, 0), Quaternion.identity);
                //hallway.transform.localScale = new Vector3(0, 2);
                for (int k = (index / width) * 18; k < 1 + (current / width) * 18; k++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        map[j + ((current % width) * 18), k] = 1;
                        Instantiate(floor, new Vector3(j + ((current % width) * 18), k, 0), Quaternion.identity);
                    }
                }
            }
            else
            {
                for (int k = (current / width) * 18; k < 2 + (index / width) * 18; k++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        map[j + ((current % width) * 18), k] = 1;
                        Instantiate(floor, new Vector3(j + ((current % width) * 18), k, 0), Quaternion.identity);
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
                        Instantiate(floor, new Vector3(k, j + end, 0), Quaternion.identity);
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
                        Instantiate(floor, new Vector3(k, j + end, 0), Quaternion.identity);
                    }
                }
            }
            index = after;
        }
    }
}
