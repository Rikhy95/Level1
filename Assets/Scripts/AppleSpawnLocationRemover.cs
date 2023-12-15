using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawnLocationRemover : MonoBehaviour
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
        gridManager.RemoveAppleSpawnPoint(position);
        Destroy(gameObject);
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
