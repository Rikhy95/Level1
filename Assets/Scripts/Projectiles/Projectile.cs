using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    #region Public variables

    #endregion

    #region Private variables
    protected GridManager gridManager;
    protected Transform snake;
    #endregion

    #region Lifecyle
    protected virtual void Start()
    {
        Vector2Int currentPosition = ExtensionMethods.ConvertToVector2Int(transform.position);
        gridManager.SetCellContentGameObject(currentPosition, gameObject);
        gridManager.UpdateCellContent(CellContent.Projectile, currentPosition);
    }
    #endregion

    #region Public methods

    public void DestroyProjectile()
    {
        Vector2Int currentPosition = ExtensionMethods.ConvertToVector2Int(transform.position);
        gridManager.ClearCellContentGameObject(currentPosition);
        gridManager.UpdateCellContent(CellContent.Empty, currentPosition);
        Destroy(gameObject);
    }
    #endregion

    #region Private methods
    protected void Move(Vector2Int newPosition)
    {
        if (newPosition.x > gridManager.width - 1 || newPosition.x < 0 || newPosition.y > gridManager.height - 1 || newPosition.y < 0)
        {
            DestroyProjectile();
            return;
        }
        switch (gridManager.GetCellContent(newPosition))
        {
            case CellContent.Empty:
                ApplyMovement(newPosition);
                break;
            case CellContent.SnakeHead:
                snake.GetComponent<Snake>().Damage();
                DestroyProjectile();
                break;
            case CellContent.SnakeTail:
            case CellContent.Wall:
            case CellContent.Projectile:
            case CellContent.Enemy:
                DestroyProjectile();
                break;

        }
    }

    protected void ApplyMovement(Vector2Int newPosition)
    {
        gridManager.UpdateCellContent(CellContent.Empty, ExtensionMethods.ConvertToVector2Int(transform.position));
        gridManager.ClearCellContentGameObject(ExtensionMethods.ConvertToVector2Int(transform.position));
        transform.position = ExtensionMethods.ConvertToVector3(newPosition);
        gridManager.UpdateCellContent(CellContent.Projectile, newPosition);
        gridManager.SetCellContentGameObject(newPosition, gameObject);
    }
    #endregion
}
