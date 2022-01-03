using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalHelper 
{
    public enum Perspective
    {
        firstPerson,
        thirdPerson,
        sideScroller,
        topDown
    }

    public static bool IsPaused()
    {
        bool value = false;

        if(Time.timeScale == 0)
        {
            value = true;
        }

        return value;
    }

    public static void TestEvent(string message)
    {
        Debug.Log(message);
    }
}
