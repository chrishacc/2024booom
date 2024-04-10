using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calc
{
    public static bool OnInterval(float val, float prevVal, float interval)
    {
        return (int)(prevVal / interval) != (int)(val / interval);
    }
}
