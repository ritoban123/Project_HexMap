/* TimerTest.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{

    private void Start()
    {
        Timer t = null;
        t = new Timer(
            2f,
            (dt) =>
                {
                    Debug.Log(t.TotalTimeElapsed);
                },
            () =>
                {
                    Debug.Log("2 seconds elapsed");
                    t.Reset();
                }
            );
    }
}
