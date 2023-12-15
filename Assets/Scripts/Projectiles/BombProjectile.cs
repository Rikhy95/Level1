using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BombProjectile : Projectile
{
    #region Public variables
    public Color flashingColor = Color.white;
    public float startFlashDelay = 0.5f;       //How many flashes per second at the start
    public float endFlashDelay = 0.1f;         //How many flashes per second at the end
    public GameObject explosionEffectPrefab;
    #endregion

    #region Private variables
    private int range;
    private float detonationTime;
    private float startTime;
    private Renderer rend;
    private MaterialPropertyBlock originalPropertyBlock;
    private MaterialPropertyBlock newPropertyBlock;
    private float flashTimer;
    private bool flashing = false;
    #endregion

    #region Lifecyle
    protected override void Start()
    {
        base.Start();
        startTime = Time.time;
        rend = GetComponent<Renderer>();
        originalPropertyBlock = new MaterialPropertyBlock();
        newPropertyBlock = new MaterialPropertyBlock();
        originalPropertyBlock.SetColor("_Color", rend.material.color);
        newPropertyBlock.SetColor("_Color", flashingColor);
    }

    void Update()
    {
        if (Time.time - startTime >= detonationTime)
        {
            Explode();
            return;
        }

        float lerpValue = Mathf.InverseLerp(0f, detonationTime, Time.time - startTime);
        float flashInterval = Mathf.Lerp(startFlashDelay, endFlashDelay, lerpValue);

        if (Time.time - flashTimer >= flashInterval)
        {
            // Toggle between color1 and color2
            if (!flashing)
            {
                rend.SetPropertyBlock(newPropertyBlock);
                flashing = true;
            }
            else
            {
                rend.SetPropertyBlock(originalPropertyBlock);
                flashing = false;
            }

            flashTimer = Time.time;
        }
        

    }
    #endregion

    #region Public methods
    public void SetInitialValues(GridManager gridManager, Transform snake, int range, float detonationTime)
    {
        this.gridManager = gridManager;
        this.snake = snake;
        this.range = range;
        this.detonationTime = detonationTime;
    }
    #endregion

    #region Private methods
    public void Explode()
    {
        int posX = Mathf.RoundToInt(transform.position.x);
        int posY = Mathf.RoundToInt(transform.position.y);

        int minX = Mathf.Clamp(posX - range, 0, gridManager.width - 1);
        int maxX = Mathf.Clamp(posX + range, 0, gridManager.width - 1);
        int minY = Mathf.Clamp(posY - range, 0, gridManager.height - 1);
        int maxY = Mathf.Clamp(posY + range, 0, gridManager.height - 1);
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (gridManager.GetCellContent(new Vector2Int(x, y)) == CellContent.SnakeHead)
                {
                    snake.GetComponent<Snake>().Damage();
                }
            }
        }

        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + Vector3.back * 5, Quaternion.identity);
        explosionEffect.GetComponent<ExplosionEffect>().SetSize(range);

        DestroyProjectile();
    }
    #endregion
}
