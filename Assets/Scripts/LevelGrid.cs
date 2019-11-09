using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid 
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObj;
    private int width;
    private int height;
    private GameObject[] foodObj;
    private Snake snake;

   public void Setup(Snake snake)
    {
        this.snake = snake;

        for (int j = 0; j < 20; j++)
        {
            //SpawnFood();

        }
    }
    

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

       // SpawnFood();


         
    }

    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(1, width), Random.Range(1, height));
        } while (snake.GetGridPosition() == foodGridPosition);

        foodGameObj = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObj.GetComponent<SpriteRenderer>().sprite = GameAssets.i.FoodSpr;
        foodGameObj.gameObject.tag = "food";
        foodGameObj.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        
    }


    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        
        foodObj = GameObject.FindGameObjectsWithTag("food");

         foreach(GameObject piece in foodObj)
         {
             if (snakeGridPosition.x == piece.transform.position.x && snakeGridPosition.y == piece.transform.position.y)
             {
                Object.Destroy(piece);
                //Debug.Log("Ate Food");
                return true;
             }
            
         }

        //Debug.Log("defaulted");
        return false;
         
        /*
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObj);
            SpawnFood();
        }
        */
    
    }
}
