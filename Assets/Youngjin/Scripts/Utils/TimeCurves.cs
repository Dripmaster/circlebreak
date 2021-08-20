using UnityEngine;

public static class TimeCurves
{
    public static float ExponentialMirrored(float t)
    {
        return 1 - Mathf.Pow((t - 1), 2);
    }
}
