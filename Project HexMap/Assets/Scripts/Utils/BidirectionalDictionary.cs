/* BidirectionalDictionary.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidirectionalDictionary<T1, T2> : Dictionary<T1, T2>
{
    public T1 this[T2 index]
    {
        get
        {
            if (!this.Any(x => x.Value.Equals(index)))
                throw new System.Collections.Generic.KeyNotFoundException();
            return this.First(x => x.Value.Equals(index)).Key;
        }
    }
}