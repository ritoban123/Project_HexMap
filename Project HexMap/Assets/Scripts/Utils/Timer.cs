/* Timer.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{


    public float TotalTimeElapsed { get; protected set; }

    protected float ExpireTime;
    protected Action<float> updateWrapper;
    protected Action expireWrapper;

    // NOTE: To create repeating timers, simply Reset the timer when it expires

    public Timer(float time, Action<float> updateAction, Action expireAction)
    {
        this.ExpireTime = time;
        this.updateWrapper =
            (dt) =>
            {
                TotalTimeElapsed += dt;

                if (TotalTimeElapsed >= ExpireTime)
                {
                    if (this.expireWrapper != null)
                    {
                        expireWrapper.Invoke();
                        UpdateManager.Instance.OnUpdate -= this.updateWrapper;
                        return;
                    }

                }
                if (updateAction != null)
                {
                    updateAction.Invoke(dt);
                }

            };
        UpdateManager.Instance.OnUpdate += this.updateWrapper;
        this.expireWrapper = expireAction;
    }

    public void Reset()
    {
        // The timer already went off, so we need to add it back to OnUpdate
        if (TotalTimeElapsed >= ExpireTime)
        {
            UpdateManager.Instance.OnUpdate += this.updateWrapper;
        }

        TotalTimeElapsed = 0f;
    }
}
