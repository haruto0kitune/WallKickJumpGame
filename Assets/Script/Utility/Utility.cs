using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Utility
{
    public static void Flip(SpriteRenderer _SpriteRenderer)
    {
        _SpriteRenderer.flipX = !_SpriteRenderer.flipX;
    }

    public static Vector3 GetUnitVector(GameObject Source, GameObject Target)
    {
        var TargetParent = Target.transform.parent;
        Target.transform.parent = Source.transform;
        var UnitVector = Target.transform.localPosition.normalized;
        Target.transform.parent = TargetParent;

        return UnitVector;
    }

    public static float DegToRad(float Degree)
    {
        return Degree * Mathf.Deg2Rad;
    }

    public static int RadToDeg(float Radian)
    {
        return (int)(Radian * Mathf.Rad2Deg);
    }

    public static Vector2 PolarToRectangular2D(int Angle, float Radius)
    {
        var x = Mathf.Ceil(Radius * Mathf.Cos(DegToRad(Angle)));
        var y = Mathf.Ceil(Radius * Mathf.Sin(DegToRad(Angle)));

        return new Vector2(x, y);
    }

    public static Vector2 RectangularToPolar2D(float x, float y)
    {
        var r = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
        var Theta = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        return new Vector2(r, Theta);
    }

    /// <summary>
    /// Compress adjacently repeated parameter  
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="seq">Sequence</param>
    /// <returns>Returns the sequence compressed adjacently repeated parameter</returns>
    public static IEnumerable<T> DistinctAdjacently<T>(this IEnumerable<T> seq)
    {
        T prev = default(T);

        foreach (var x in seq)
        {
            if (prev == null || !prev.Equals(x))
            {
                yield return x;
            }

            prev = x;
        }
    }

    public static List<float> Ease(float startPoint, float endPoint, int t, int maxTime)
    {
        var distance = endPoint - startPoint;
        var midpoint = (distance / 2) + startPoint;

        var easeOut = EaseOut(startPoint, midpoint, t, maxTime);
        var easeIn = EaseIn(midpoint, endPoint, t, maxTime);

        return easeOut.Concat(easeIn).ToList();
    }

    public static List<float> EaseOut(float startPoint, float endPoint, int t, int maxTime)
    {
        var list = new List<float>();

        for (int i = t; i < maxTime; i++)
        {
            list.Add(startPoint + (endPoint - startPoint) * ((i + 1) * i / 2.0f) / ((maxTime + 1) * maxTime / 2.0f));
        }

        return list; 
    }

    public static List<float> EaseIn(float startPoint, float endPoint, int t, int maxTime)
    {
        var list = new List<float>();

        for (int i = t; i < maxTime; i++)
        {
            list.Add(startPoint + (endPoint - startPoint) * ((maxTime * 2 - i + 1) * i / 2.0f) / ((maxTime + 1) * maxTime / 2.0f));
        }

        return list;
    }
}