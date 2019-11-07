using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosition;
    private Vector2Int gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private GameObject snakeBodyObj;
    private List<Vector2Int> snakeMovePositionList;
    private GameObject[] snkObj;
    
    
    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    

   private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 0;
    }

    private void Update()
    {


        HandleInput();
        HandleGridMovement();
        

    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && gridMoveDirection.y != -1)
        {
            gridMoveDirection.y = 1;
            gridMoveDirection.x = 0;

            snakeMovePositionList.Insert(0, gridPosition);
            gridPosition += gridMoveDirection;
            gridMoveTimer = 0;
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && gridMoveDirection.y != 1)
        {
            gridMoveDirection.y = -1;
            gridMoveDirection.x = 0;

            snakeMovePositionList.Insert(0, gridPosition);
            gridPosition += gridMoveDirection;
            gridMoveTimer = 0;
           
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && gridMoveDirection.x != 1)
        {
            gridMoveDirection.x = -1;
            gridMoveDirection.y = 0;

            snakeMovePositionList.Insert(0, gridPosition);
            gridPosition += gridMoveDirection;
            gridMoveTimer = 0;
           

        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && gridMoveDirection.x != -1)
        {
            gridMoveDirection.x = 1;
            gridMoveDirection.y = 0;

            snakeMovePositionList.Insert(0, gridPosition);
            gridPosition += gridMoveDirection;
            gridMoveTimer = 0;
           
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            

          
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;
        }

       bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
        Debug.Log("snakeAteFood is " + snakeAteFood);
        if (snakeAteFood)
        {
            Debug.Log("I am growing");
            snakeBodySize++;
        }
        if (snakeMovePositionList.Count >= snakeBodySize + 1)
        {
           // Debug.Log("snake too long");
            Vector2Int lastSnakeMovePosition = snakeMovePositionList[snakeMovePositionList.Count - 1];
            snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            snkObj = GameObject.FindGameObjectsWithTag("body");

            foreach(GameObject part in snkObj)
            {
                if(lastSnakeMovePosition.x == part.transform.position.x && lastSnakeMovePosition.y == part.transform.position.y)
                {
                    Object.Destroy(part);
                }
            }
            
        }

        for(int j = 0; j < snakeMovePositionList.Count; j++)
        {
            //Debug.Log("Make snake body");
            Vector2Int snakeMovePosition = snakeMovePositionList[j];
            snkObj = GameObject.FindGameObjectsWithTag("body");

            foreach (GameObject part in snkObj)
            {
                if (snakeMovePosition.x == part.transform.position.x && snakeMovePosition.y == part.transform.position.y)
                {
                    Object.Destroy(part);
                }
            }
            snakeBodyObj = new GameObject("Snake Body", typeof(SpriteRenderer));
            snakeBodyObj.GetComponent<SpriteRenderer>().sprite = GameAssets.i.SnakeBodySpr;
            snakeBodyObj.gameObject.tag = "body";
            snakeBodyObj.transform.position = new Vector3(snakeMovePosition.x, snakeMovePosition.y);
            snakeBodyObj.transform.localScale = Vector3.one;
            
            //GameObject.Destroy(snakeBodyObj);


        }




        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);

        
        
        

        /*for (int i = 0; i < snakeMovePositionList.Count; i++)
        {
            Vector2Int snakeMovePosition = snakeMovePositionList[i];
            snakeBodyObj = new GameObject("Snake Body", typeof(SpriteRenderer));
            snakeBodyObj.GetComponent<SpriteRenderer>().sprite = GameAssets.i.SnakeBodySpr;
            snakeBodyObj.transform.position = new Vector3(snakeMovePosition.x, snakeMovePosition.y);
            GameObject.Destroy(snakeBodyObj);

        }*/
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
}
