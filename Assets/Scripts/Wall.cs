using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    #region Public variables
    public GridManager gridManager;
    #endregion

    #region Private variables
    
    #endregion

    #region Lifecyle
    void Start()
    {
        Vector2Int position = ExtensionMethods.ConvertToVector2Int(transform.position);
        transform.position = ExtensionMethods.ConvertToVector3(position);
        gridManager.UpdateCellContent(CellContent.Wall, position);

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
