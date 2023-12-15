using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : Projectile
{
    #region Public variables

    #endregion

    #region Private variables
    private Vector2Int direction;
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
            Move(ExtensionMethods.ConvertToVector2Int(transform.position) + direction);
        }
    }
    #endregion

    #region Public methods
    
    public void SetInitialValues(Vector2Int direction, float moveDelay, GridManager gridManager, Transform snake)
    {
        this.direction = direction;
        this.moveDelay = moveDelay;
        this.gridManager = gridManager;
        this.snake = snake;
    }

    #endregion

    #region Private methods

    #endregion
}
