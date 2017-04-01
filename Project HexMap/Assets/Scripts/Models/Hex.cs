/* Hex.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{
    public Vector3 Position { get; protected set; }
    public Hex(Vector3 position)
    {
        Position = position;
    }

}