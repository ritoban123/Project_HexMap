/* HexMapLayout.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapLayout
{
    public HexOrientation Orientation { get; protected set; }
    public Vector2 HexSize;
    public Vector2 Origin;

    public HexMapLayout(HexOrientation orientation, Vector2 hexSize, Vector2 origin)
    {
        Orientation = orientation;
        HexSize = hexSize;
        Origin = origin;
    }
}
