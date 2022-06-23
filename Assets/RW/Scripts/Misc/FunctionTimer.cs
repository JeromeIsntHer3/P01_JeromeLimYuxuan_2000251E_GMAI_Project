using System;
using UnityEngine;

//This class is timer that takes in a function and a time
//and then runs the function after that time has finished counting down
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