using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomingProjectile : Projectile
{
    #region Public variables

    #endregion

    #region Private variables
    private float moveDelay;
    private float lastMovementTime;

    #endregion

    #region Lifecyle
    protected override void Start()
    {
        base.Start();
        lastMovementTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastMovementTime > moveDelay)
        {
            lastMovementTime += moveDelay;
            Move(ExtensionMethods.ConvertToVector2Int(transform.position) + CalculateNewDirection());
        }
    }
    #endregion

    #region Public methods
    public void SetInitialValues(float moveDelay, GridManager gridManager, Transform snake)
    {
        this.moveDelay = moveDelay;
        this.gridManager = gridManager;
        this.snake = snake;
    }
    #endregion

    #region Private methods
    
    private Vector2Int CalculateNewDirection()
    {
        // Calculate the difference in grid positions
        Vector2Int gridDifference = new Vector2Int(
            Mathf.RoundToInt(snake.transform.position.x - transform.position.x),
            Mathf.RoundToInt(snake.transform.position.y - transform.position.y)
        );

        // Determine the primary movement direction
        Vector2Int moveDirection = Vector2Int.zero;

        if (Mathf.Abs(gridDifference.x) > Mathf.Abs(gridDifference.y))
        {
            moveDirection.x = Mathf.Clamp(gridDifference.x, -1, 1);
        }
        else if (Mathf.Abs(gridDifference.y) > Mathf.Abs(gridDifference.x))
        {
            moveDirection.y = Mathf.Clamp(gridDifference.y, -1, 1);
        }
        else
        {
            // Randomly choose either x or y when distances are equal
            if (Random.Range(0, 2) == 0)
            {
                moveDirection.x = Mathf.Clamp(gridDifference.x, -1, 1);
            }
            else
            {
                moveDirection.y = Mathf.Clamp(gridDifference.y, -1, 1);
            }
        }

        return moveDirection;
    }
    #endregion
}
