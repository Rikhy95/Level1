using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type2Enemy : Enemy
{
    #region Public variables
    public GameObject homingProjectilePrefab;
    public float projectileMoveDelay;

    #endregion

    #region Private variables

    #endregion

    #region Lifecyle
    protected override void Update()
    {
        base.Update();
        if (Time.time - lastShootingTime > currentShootingDelay)
        {
            lastShootingTime += currentShootingDelay;
            CalculateNewShootingDelay();
            try
            {
                ShootHoming();
            }
            catch
            {
                //Debug.LogWarning("Projectile spawn point was outside the grid");
            }
        }
    }
    #endregion

    #region Public methods
    private void ShootHoming()
    {
        Vector2Int direction = CalculateHomingDirection();
        Vector2Int spawnPosition = ExtensionMethods.ConvertToVector2Int(transform.position) + direction;

        if (gridManager.GetCellContent(spawnPosition) == CellContent.Empty)
        {
            GameObject projectile = Instantiate(homingProjectilePrefab, ExtensionMethods.ConvertToVector3(spawnPosition), Quaternion.identity);
            HomingProjectile projectileScript = projectile.GetComponent<HomingProjectile>();
            projectileScript.SetInitialValues(projectileMoveDelay, gridManager, snake);
        }
    }

    private Vector2Int CalculateHomingDirection()
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
            moveDirection = snake.GetComponent<Snake>().GetCurrentDirection();
        }

        return moveDirection;
    }
    #endregion

    #region Private methods

    #endregion
}
