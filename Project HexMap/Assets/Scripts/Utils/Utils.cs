using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static Vector3 WorldPosToHexCoord(this Vector3 p, HexMapLayout layout)
    {
        HexOrientation M = layout.Orientation;
        Vector2 pt = new Vector2    ((p.x - layout.Origin.x) / layout.HexSize.x,
                     (p.z - layout.Origin.y) / layout.HexSize.y);
        float q = M.b0 * pt.x + M.b1 * pt.y;
        float r = M.b2 * pt.x + M.b3 * pt.y;
        return new Vector3(q, r, -q - r);
    }
}
