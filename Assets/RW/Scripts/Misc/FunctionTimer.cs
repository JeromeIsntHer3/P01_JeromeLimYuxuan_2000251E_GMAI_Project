using System;
using UnityEngine;

public class FunctionTimer
{
    private Action action;
    private float timer;
    private bool isDestroyed;
    
    public FunctionTimer(Action action,float timer)
    {
        this.action = action;
        this.timer = timer;
    }
    
    public void Update()
    {
        if (!isDestroyed) 
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                action();
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        isDestroyed = true;
    }
}