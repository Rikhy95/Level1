using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ExtensionMethods
{
    public static Vector2Int WrapVector2Int(Vector2Int vector, int xMax, int yMax)
    {
        return new Vector2Int(
            (xMax + vector.x % xMax) % xMax,
            (yMax + vector.y % yMax) % yMax
            );
    }

    public static int WrapIntValue(int value, int max)
    {
        return (max + value % max) % max;
    }

    public static Vector2Int ConvertToVector2Int(Vector3 v3)
    {
        Vector3Int intTemp = Vector3Int.RoundToInt(v3);
        return new Vector2Int(intTemp.x, intTemp.y);
    }

    public static Vector3 ConvertToVector3(Vector2Int v2i)
    {
        return new Vector3(v2i.x, v2i.y, 0);
    }
}
