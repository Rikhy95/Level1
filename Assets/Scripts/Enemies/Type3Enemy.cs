using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Type3Enemy : Enemy
{
    #region Public variables
    public GameObject bombPrefab;

    public int bombRange;
    public int bombDetonationTime;
    #endregion

    #region Private variables
    private Vector2Int[] bombSpawnPoints;
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
                ThrowBomb(GetBombSpawnPoint());
            }
            catch
            {
                //Debug.LogWarning("Projectile spawn point was outside the grid");
            }
        }
    }
    #endregion

    #region Public methods
    public void SetBombSpawnPoints(Vector2Int[] points)
    {
        bombSpawnPoints = points;
    }
    #endregion

    #region Private methods
    private void ThrowBomb(Vector2Int spawnPosition)
    {
        if (gridManager.GetCellContent(spawnPosition) == CellContent.Empty)
        {
            GameObject projectile = Instantiate(bombPrefab, ExtensionMethods.ConvertToVector3(spawnPosition), Quaternion.identity);
            BombProjectile projectileScript = projectile.GetComponent<BombProjectile>();
            projectileScript.SetInitialValues(gridManager, snake, bombRange, bombDetonationTime);
        }
    }

    private Vector2Int GetBombSpawnPoint()
    {
        return bombSpawnPoints[Random.Range(0, bombSpawnPoints.Length)];
    }
    #endregion
}
