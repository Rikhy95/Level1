using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsGenerator : MonoBehaviour
{
    #region Public variables
    public GridManager gridManager;
    public GameObject cell1;
    public GameObject cell2;
    public int zOffset = 5;
    #endregion

    #region Private variables
    
    #endregion

    #region Lifecyle
    void Start()
    {
        GameObject newCell;
        for (int y = 0; y < gridManager.height; y++)
        {
            for (int x = 0; x < gridManager.width; x++)
            {
                if ((x + y) % 2 == 0)
                {
                    newCell = Instantiate(cell1, transform);
                }
                else
                {
                    newCell = Instantiate(cell2, transform);
                }
                newCell.transform.localPosition = new Vector3(x, y, zOffset);
            }
        }
    }

    void Update()
    {
        
    }
    #endregion

    #region Public methods
    
    #endregion

    #region Private methods
    
    #endregion
}
