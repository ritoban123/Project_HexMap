using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // FIXME: use the one in the HexCoord class
    //public static Vector3 WorldPosToHexCoord(this Vector3 p, HexMapLayout layout)
    //{
    //    HexOrientation M = layout.Orientation;
    //    Vector2 pt = new Vector2    ((p.x - layout.Origin.x) / layout.HexSize.x,
    //                 (p.z - layout.Origin.y) / layout.HexSize.y);
    //    float q = M.b0 * pt.x + M.b1 * pt.y;
    //    float r = M.b2 * pt.x + M.b3 * pt.y;
    //    return new Vector3(q, r, -q - r);
    //}

    public static HexCoord RoundHex(this Vector3 h)
    {
        int q = Mathf.RoundToInt(h.x);
        int r = Mathf.RoundToInt(h.y);
        int s = Mathf.RoundToInt(h.z);
        double q_diff = Mathf.Abs(q - h.x);
        double r_diff = Mathf.Abs(r - h.y);
        double s_diff = Mathf.Abs(s - h.z);
        if (q_diff > r_diff && q_diff > s_diff) {
            q = -r - s;
        } else if (r_diff > s_diff)
        {
            r = -q - s;
        }
        else
        {
            s = -q - r;
        }
        return new HexCoord(q, r, s);
    }

    public static float RoundToNearestHalf(this float a)
    {
        return a = Mathf.Round(a * 2f) * 0.5f;
    }

    public static Vector3 RoundToNearestHalf(this Vector3 a)
    {
        return new Vector3(a.x.RoundToNearestHalf(), a.y.RoundToNearestHalf(), a.z.RoundToNearestHalf());
    }
}
