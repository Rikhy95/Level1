using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1Enemy : Enemy
{
    #region Public variables
    public GameObject projectilePrefab;
    public float projectileMoveDelay;

    [Header("Available shooting directions")]
    public bool shootDirectionUp = false;
    public bool shootDirectionDown = false;
    public bool shootDirectionLeft = false;
    public bool shootDirectionRight = false;

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
                ShootProjectile();
            }
            catch
            {
                //Debug.LogWarning("Projectile spawn point was outside the grid");
            }
        }
    }
    #endregion

    #region Public methods

    #endregion

    #region Private methods
    private void ShootProjectile()
    {
        if (shootDirectionUp)
        {
            ShootStraightProjectile(Vector2Int.up);
        }
        if (shootDirectionDown)
        {
            ShootStraightProjectile(Vector2Int.down);
        }
        if (shootDirectionLeft)
        {
            ShootStraightProjectile(Vector2Int.left);
        }
        if (shootDirectionRight)
        {
            ShootStraightProjectile(Vector2Int.right);
        }
    }

    private void ShootStraightProjectile(Vector2Int direction)
    {
        Vector2Int spawnPosition = ExtensionMethods.ConvertToVector2Int(transform.position) + direction;
        if (gridManager.GetCellContent(spawnPosition) == CellContent.Empty)
        {
            GameObject projectile = Instantiate(projectilePrefab, ExtensionMethods.ConvertToVector3(spawnPosition), Quaternion.identity);
            StraightProjectile projectileScript = projectile.GetComponent<StraightProjectile>();
            projectileScript.SetInitialValues(direction, projectileMoveDelay, gridManager, snake);
        }
    }
    #endregion
}
