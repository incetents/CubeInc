﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    public static bool LineIntersectAABB(Vector2 lineP1, Vector2 lineP2, Vector2 min, Vector2 max)
    {
        Debug.Log(lineP1 + " " + lineP2 + " " + min + " " + max);
        Vector2 origin = lineP1;
        Vector2 direction = lineP2 - lineP1;

        // If either points are in the box, also counts as intersecting
        if (
            (lineP1.x >= min.x && lineP1.y >= min.y && lineP1.x <= max.x && lineP1.y <= max.y) ||
            (lineP2.x >= min.x && lineP2.y >= min.y && lineP2.x <= max.x && lineP2.y <= max.y)
            )
            return true;

        // Vertical Line
        if(direction.x == 0)
        {
            return (origin.x >= min.x && origin.x <= max.x);
        }
        // Horizontal line
        else if(direction.y == 0)
        {
            return (origin.y >= min.y && origin.y <= max.y);
        }

        float tmin = (min.x - origin.x) / direction.x;
        float tmax = (max.x - origin.x) / direction.x;

       

        if (tmin > tmax)
            Swap(ref tmin, ref tmax);

        float tymin = (min.y - origin.y) / direction.y;
        float tymax = (max.y - origin.y) / direction.y;

        if (tymin > tymax)
            Swap(ref tymin, ref tymax);

        if ((tmin > tymax) || (tymin > tmax))
            return false;

        if (tymin > tmin)
            tmin = tymin;

        if (tymax < tmax)
            tmax = tymax;

        Vector2 hitLine = new Vector2(tymin, tymax) - origin;
        // If hit line is longer than drawn line, detection fails
        return (hitLine.magnitude <= direction.magnitude);
    }

    // in Vector3Int
    public static int ManhattanDistance(this Vector3Int a, Vector3Int b)
    {
        checked
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
        }
    }
    // in Vector2Int
    public static int ManhattanDistance(this Vector2Int a, Vector2Int b)
    {
        checked
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }

}