using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Serializable]
    public struct CellData
    {
        public CellContent content;
        public bool hasApple;
        public GameObject contentGameObject;
        public GameObject appleGameObject;
        public bool canHaveApple;
    }

    #region Public variables
    public int height;
    public int width;
    public GameObject applePrefab;
    #endregion

    #region Private variables
    private static CellData[,] grid;
    private bool firstAppleGenerated = false;
    #endregion

    #region Lifecyle
    void Awake()
    {
        InitializeGrid();
    }

    void Update()
    {
        if (Time.time > 0.1f && !firstAppleGenerated)
        {
            firstAppleGenerated = true;
            GenerateNewApple();
            
        }
        //PrintGrid();
    }
    #endregion

    #region Public methods
    public void UpdateCellContent(CellContent newContent, Vector2Int coordinates)
    {
        if (grid != null)
        {
            grid[coordinates.x, coordinates.y].content = newContent;
        }
        else
        {
            Debug.LogWarning("Grid does not exist");
        }
    }

    public CellContent GetCellContent(Vector2Int coordinates)
    {
        return grid[coordinates.x, coordinates.y].content;
    }

    public GameObject GetCellContentGameObject(Vector2Int coordinates)
    {
        return grid[coordinates.x, coordinates.y].contentGameObject;
    }

    public bool GetIfCellHasApple(Vector2Int coordinates, out GameObject apple)
    {
        apple = grid[coordinates.x, coordinates.y].appleGameObject;
        return grid[coordinates.x, coordinates.y].hasApple;
    }

    public void AppleAte(Vector2Int coordinates)
    {
        grid[coordinates.x, coordinates.y].hasApple = false;
        GenerateNewApple();
    }

    public void RemoveAppleSpawnPoint(Vector2Int coordinates)
    {
        grid[coordinates.x, coordinates.y].canHaveApple = false;
    }

    public void ClearCellContentGameObject (Vector2Int coordinates)
    {
        grid[coordinates.x, coordinates.y].contentGameObject = null;
    }

    public void SetCellContentGameObject(Vector2Int coordinates, GameObject instance)
    {
        grid[coordinates.x, coordinates.y].contentGameObject = instance;
    }

    #endregion

    #region Private methods
    private void InitializeGrid()
    {
        grid = new CellData[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y].canHaveApple = true;
            }
        }
    }

    private void GenerateNewApple()
    {
        List<Vector2Int> availableSpots = new List<Vector2Int>();
        
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0;x < grid.GetLength(0); x++)
            {
                if (grid[x, y].content == CellContent.Empty && grid[x, y].canHaveApple)
                {
                    availableSpots.Add(new Vector2Int(x, y));
                }
            }
        }

        int selectedCell = UnityEngine.Random.Range(0, availableSpots.Count);
        Vector2Int newAppleSpot = availableSpots[selectedCell];

        grid[newAppleSpot.x, newAppleSpot.y].hasApple = true;
        grid[newAppleSpot.x, newAppleSpot.y].appleGameObject = Instantiate(applePrefab, ExtensionMethods.ConvertToVector3(newAppleSpot), Quaternion.identity);
    }

    private void GenerateNewApple(Vector2Int position)
    {
        
        Vector2Int newAppleSpot = position;

        grid[newAppleSpot.x, newAppleSpot.y].hasApple = true;
        grid[newAppleSpot.x, newAppleSpot.y].appleGameObject = Instantiate(applePrefab, ExtensionMethods.ConvertToVector3(newAppleSpot), Quaternion.identity);
    }
    #endregion

    #region Public static methods
    [MenuItem("Debug/Print Grid")]
    public static void PrintGrid()
    {
        Debug.Log("-------------------------------------------------------------------");
        for (int i = grid.GetLength(1) - 1; i >= 0; i--)
        {
            string prompt = "";
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                switch (grid[j, i].content)
                {
                    case CellContent.Empty:
                        prompt += "<color=black>■</color>";
                        break;
                    case CellContent.SnakeHead:
                        prompt += "<color=red>■</color>";
                        break;
                    case CellContent.SnakeTail:
                        prompt += "<color=white>■</color>";
                        break;
                    case CellContent.Wall:
                        prompt += "<color=brown>■</color>";
                        break;
                    case CellContent.Projectile:
                        prompt += "<color=yellow>■</color>";
                        break;
                    case CellContent.Enemy:
                        prompt += "<color=blue>■</color>";
                        break;

                }
            }
            Debug.Log($"{prompt}");
        }
    }
    #endregion
}
