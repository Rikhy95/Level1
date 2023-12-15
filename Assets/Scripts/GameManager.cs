using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    #region Public variables
    public Snake snake;
    public List<Enemy> enemies;
    public GridManager gridManager;
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public BombSpawnPoints bombSpawnPointsInterface;
    #endregion

    #region Private variables
    private bool gameOver = false;
    private List<Vector2Int> bombSpawnPoints = new List<Vector2Int>();
    private bool paused = false;
    #endregion

    #region Lifecyle
    void Start()
    {
        snake.SetGridManager(gridManager);
        snake.gameOver += GameOver;

        CopyBombSpawnPoints(bombSpawnPointsInterface.spawnPoints);

        if (enemies.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.SetInitialValues(snake.transform, gridManager);
                    enemy.destroyed += HandleEnemyDestroyed;

                    if (enemy is Type3Enemy)
                    {
                        Type3Enemy type3Enemy = (Type3Enemy)enemy;
                        type3Enemy.SetBombSpawnPoints(bombSpawnPoints.ToArray());
                    }
                }
                else
                {
                    Debug.LogWarning("GAME MANAGER HAS EMPTY ENEMY ENTRIES");
                }
            }
        }
        else
        {
            Debug.LogWarning("THERE ARE NO ENEMIES");
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        if (enemies.Count == 0 && !gameOver)
        {
            GameOver(victory:true);
        }
    }
    #endregion

    #region Public methods
    
    #endregion

    #region Private methods
    private void CopyBombSpawnPoints(Vector2Int[] spawnPointsList)
    {
        for (int i = 0; i < spawnPointsList.Length; i++)
        {
            if (spawnPointsList[i].x >= 0
                &&
                spawnPointsList[i].x < gridManager.width
                &&
                spawnPointsList[i].y >= 0
                &&
                spawnPointsList[i].y < gridManager.height
               )
            {
                bombSpawnPoints.Add(spawnPointsList[i]);
            }
        }

    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemy.destroyed -= HandleEnemyDestroyed;
        enemies.RemoveAll(element => element == enemy);
    }

    private void GameOver(bool victory = false)
    {
        Time.timeScale = 0f;
        gameOver = true;
        if (victory)
        {
            winScreen.SetActive(true);
        }
        else
        {
            gameOverScreen.SetActive(true);
        }
    }

    private void Pause()
    {
        if (!gameOver)
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            paused = true;
        }
    }

    private void Unpause()
    {
        if (!gameOver)
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            paused = false;
        }
    }
    #endregion
}
