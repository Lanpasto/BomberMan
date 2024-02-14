using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGeneratePattern : MonoBehaviour
{
    public enum RandomTypeEnum
    {   
        Default,
        IsOnLine,
        PatternX,
        PatternDiagonal,

    }
   public static int GetRandomIndex()
{
    System.Random random = new System.Random();
    int randomIndex = random.Next(2);
    return randomIndex;
}

    public static float GetSpawnChance()
    {
        return Random.Range(0f, 1f);
    }
    public static bool IsOnDiagonalPatternFromLeftTopToRightBottom(Vector3 coordinate, float tolerance)
    {
        return Mathf.Abs(coordinate.x - coordinate.y) <= tolerance;
    }
    public static bool IsOnDiagonalsPatternX(Vector3 coordinate, float width, float height, float tolerance)
    {
        float centerX = width / 2f;
        float centerY = height / 2f;
        float distanceX = Mathf.Abs(coordinate.x - centerX);
        float distanceY = Mathf.Abs(coordinate.y - centerY);
        return Mathf.Abs(distanceX - distanceY) <= tolerance;
    }

    public static bool IsOnLine(Vector3 coordinate, float width, float height, float tolerance, bool horizontal)
    {
        float centerX = width / 2f;
        float centerY = height / 2f;

        if (horizontal)
        {
            float distanceY = Mathf.Abs(coordinate.y - centerY);
            return distanceY <= tolerance;
        }
        else
        {
            float distanceX = Mathf.Abs(coordinate.x - centerX);
            return distanceX <= tolerance;
        }
    }
}
