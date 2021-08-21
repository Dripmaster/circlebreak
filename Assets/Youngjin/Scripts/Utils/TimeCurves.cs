using UnityEngine;

public delegate float TimeCurve(float t);
public static class TimeCurves
{
    public static float ExponentialMirrored(float t)
    {
        return 1 - Mathf.Pow((t - 1), 2);
    }
    public static float Exponential(float t)
    {
        return Mathf.Pow(t, 2);
    }
    /*public static float ExponentialPingPong(float t)
    {

    }*/
}
