using System;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string Timer(float time)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(time);
        string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
        return timer;
    }
}
