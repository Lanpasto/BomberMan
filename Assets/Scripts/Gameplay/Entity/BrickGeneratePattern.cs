using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGeneratePattern : MonoBehaviour
{
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
    public static bool IsOnDiagonalPatternRightBottom(Vector3 coordinate, int width, int height, float tolerance)// \
    {
        float diagonalLength = Mathf.Sqrt(width * width + height * height);
        float distanceToBottomLeft = Vector2.Distance(coordinate, new Vector2(0, height));
        float distanceToTopRight = Vector2.Distance(coordinate, new Vector2(width, 0));

        return Mathf.Abs(distanceToBottomLeft + distanceToTopRight - diagonalLength) <= tolerance;
    }

    public static bool IsOnDiagonalsPatternRightTop(Vector3 coordinate, int width, int height, float tolerance)// /
    {
        float diagonalLength = Mathf.Sqrt(width * width + height * height);
        float distanceToTopLeft = Vector2.Distance(coordinate, Vector2.zero);
        float distanceToBottomRight = Vector2.Distance(coordinate, new Vector2(width, height));

        return Mathf.Abs(distanceToTopLeft + distanceToBottomRight - diagonalLength) <= tolerance;
    }
    public static bool IsOnDiagonalsPatternX(Vector3 coordinate, int width, int height, float tolerance)
    {
        float diagonalLength = Mathf.Sqrt(width * width + height * height);
        float distanceToBottomLeft = Vector2.Distance(coordinate, new Vector2(0, height));
        float distanceToTopRight = Vector2.Distance(coordinate, new Vector2(width, 0));
        float distanceToTopLeft = Vector2.Distance(coordinate, Vector2.zero);
        float distanceToBottomRight = Vector2.Distance(coordinate, new Vector2(width, height));

        bool onDiagonalX1 = Mathf.Abs(distanceToBottomLeft + distanceToTopRight - diagonalLength) <= tolerance;
        bool onDiagonalX2 = Mathf.Abs(distanceToTopLeft + distanceToBottomRight - diagonalLength) <= tolerance;

        return onDiagonalX1 || onDiagonalX2;
    }

    public static bool IsOnLine(Vector3 coordinate, float width, float height, float tolerance)
    {
        float centerX = width / 2f;
        float centerY = height / 2f;

        float distanceX = Mathf.Abs(coordinate.x - centerX);
        float distanceY = Mathf.Abs(coordinate.y - centerY);

        bool closerToHorizontalLine = distanceY <= tolerance*2f;
        bool closerToVerticalLine = distanceX <= tolerance*2f;

        if (Mathf.Approximately(distanceX, distanceY))
        {
            return false;
        }

        if (closerToHorizontalLine)
        {
            return true;
        }
        else if (closerToVerticalLine)
        {
            return true;
        }

        return false;
    }


    public static float CalculateSpawnChance(float distance)
    {
        float minSpawnChance = 0.4f; // Мінімальний шанс спавну
        float maxSpawnChance = 0.6f; // Максимальний шанс спавну
        float spawnChance = maxSpawnChance - (maxSpawnChance - minSpawnChance) * distance;

        return Mathf.Clamp(spawnChance, minSpawnChance, maxSpawnChance);
    }

}

public enum RandomTypeEnum
{
    Default,
    IsOnLine,
    PatternX,
    PatternDiagonal,

}


/*
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
*/