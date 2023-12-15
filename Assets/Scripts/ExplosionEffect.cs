using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    #region Public variables
    public float timeToDisappear = 1f;
    #endregion

    #region Private variables
    private float startTime;
    private float finalAlpha = 0;
    private Color startingColor;
    private Color endColor;
    private Renderer rend;
    #endregion

    #region Lifecyle
    void Start()
    {
        rend = GetComponent<Renderer>();
        startTime = Time.time;
        startingColor = rend.material.color;
        endColor = startingColor;
        endColor.a = finalAlpha;
        SetSize(3);
    }

    void Update()
    {
        if (Time.time - startTime > timeToDisappear)
        {
            Destroy(gameObject);
        }

        float lerpValue = Mathf.InverseLerp(startTime, startTime + timeToDisappear, Time.time);
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_Color", Color.Lerp(startingColor, endColor, lerpValue));
        rend.SetPropertyBlock(materialPropertyBlock);
    }
    #endregion

    #region Public methods
    public void SetSize(int range)
    {
        Vector3 newScale = Vector3.one * (range * 2 + 1);
        newScale.z = 1;
        transform.localScale = newScale;
    }
    #endregion

    #region Private methods
    
    #endregion
}
