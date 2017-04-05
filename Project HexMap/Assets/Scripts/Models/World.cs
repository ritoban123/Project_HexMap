/* World.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public HashSet<Hex> HexMap { get; protected set; }
    
    public World()
    {
        HexMap = new HashSet<Hex>();
    }

    public void AddHex(Hex hex)
    {
        HexMap.Add(hex);
    }

    public void RemoveHex(Hex hex)
    {
        HexMap.Add(hex);
    }

}
