using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Enemy : MonoBehaviour
{
    public event System.Action<Enemy> destroyed = null;

    #region Public variables

    public int lengthToKill = 3;
    [Tooltip("Color that the enemy becomes when he is vulnerable")]
    public Color vulnerableColor = Color.magenta;
    [Header("Movement")]
    public Direction[] movementSteps;
    public float movementDelay;

    [Header("Shooting")]
    public float shootingDelayMin;
    public float shootingDelayMax;

    #endregion

    #region Private variables
    protected Transform snake;
    protected GridManager gridManager;
    protected float lastMovementTime;
    protected float lastShootingTime;
    protected float currentShootingDelay;
    protected Vector2Int[] moveCycle;
    protected int currentCycleStep = 0;
    protected bool isKnownByGameManager = false;
    private Renderer rend;
    private MaterialPropertyBlock originalPropertyBlock;
    private MaterialPropertyBlock vulnerablePropertyBlock;
    #endregion

    #region Lifecyle
    protected virtual void Start()
    {
        if (!isKnownByGameManager)
        {
            Debug.LogWarning($"[{gameObject.name}] GAME MANAGER DOES NOT KNOW ABOUT ME!");
        }
        lastMovementTime = Time.time;
        lastShootingTime = Time.time;
        rend = GetComponent<Renderer>();
        originalPropertyBlock = new MaterialPropertyBlock();
        vulnerablePropertyBlock = new MaterialPropertyBlock();
        originalPropertyBlock.SetColor("_Color", rend.material.color);
        vulnerablePropertyBlock.SetColor("_Color", vulnerableColor);
        SnapPositionAtStart();
        InitializeMoveCycle();
        CalculateNewShootingDelay();
    }

    protected virtual void Update()
    {
        if (Time.time - lastMovementTime > movementDelay)
        {
            lastMovementTime += movementDelay;
            ProcessMoveCycleStep();
        }

        if (snake.GetComponent<Snake>().GetTailLength() < lengthToKill)
        {
            rend.SetPropertyBlock(originalPropertyBlock);
        }
        else
        {
            rend.SetPropertyBlock(vulnerablePropertyBlock);
        }
    }

    #endregion

    #region Public methods
    public void SetInitialValues(Transform snake, GridManager gridManager)
    {
        isKnownByGameManager = true;
        this.snake = snake;
        this.gridManager = gridManager;
    }

    public void Kill()
    {
        if (destroyed != null)
        {
            destroyed(this);
        }
        Vector2Int currentPos = ExtensionMethods.ConvertToVector2Int(transform.position);
        gridManager.UpdateCellContent(CellContent.Empty, currentPos);
        gridManager.ClearCellContentGameObject(currentPos);
        Destroy(gameObject);
    }
    #endregion

    #region Protected methods
    protected void InitializeMoveCycle()
    {
        moveCycle = new Vector2Int[movementSteps.Length];

        for (int i = 0; i < movementSteps.Length; i++)
        {
            switch (movementSteps[i])
            {
                case Direction.Up:
                    moveCycle[i] = Vector2Int.up;
                    break;
                case Direction.Down:
                    moveCycle[i] = Vector2Int.down;
                    break;
                case Direction.Left:
                    moveCycle[i] = Vector2Int.left;
                    break;
                case Direction.Right:
                    moveCycle[i] = Vector2Int.right;
                    break;

            }
        }
    }

    protected void ProcessMoveCycleStep()
    {
        if (moveCycle.Length > 0)
        {
            Vector2Int newPosition = ExtensionMethods.ConvertToVector2Int(transform.position) + moveCycle[currentCycleStep];
            if (Move(newPosition))
            {
                currentCycleStep = ExtensionMethods.WrapIntValue(currentCycleStep + 1, moveCycle.Length);
            }
        }
    }

    protected bool Move(Vector2Int destination)
    {
        destination = ExtensionMethods.WrapVector2Int(destination, gridManager.width, gridManager.height);
        switch (gridManager.GetCellContent(destination))
        {
            case CellContent.Empty:
                ApplyMovement(destination);
                return true;
            default:
                Debug.Log($"Cell {destination} is occupied");
                return false;
        }
    }

    protected void ApplyMovement(Vector2Int newPosition)
    {
        Vector2Int oldPosition = ExtensionMethods.ConvertToVector2Int(transform.position);
        gridManager.ClearCellContentGameObject(oldPosition);
        gridManager.UpdateCellContent(CellContent.Empty, oldPosition);
        transform.position = ExtensionMethods.ConvertToVector3(newPosition);
        gridManager.UpdateCellContent(CellContent.Enemy, newPosition);
        gridManager.SetCellContentGameObject(newPosition, gameObject);
    }

    protected void SnapPositionAtStart()
    {
        Vector2Int pos = ExtensionMethods.ConvertToVector2Int(transform.position);
        transform.position = ExtensionMethods.ConvertToVector3(pos);
        gridManager.UpdateCellContent(CellContent.Enemy, pos);
        gridManager.SetCellContentGameObject(pos, gameObject);
    }

    protected void CalculateNewShootingDelay()
    {
        currentShootingDelay = Random.Range(shootingDelayMin, shootingDelayMax);
    }
    #endregion
}