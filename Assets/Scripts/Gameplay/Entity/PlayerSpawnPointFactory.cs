using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointFactory
{
    private int width;
    private int height;

    public PlayerSpawnPointFactory(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public List<Vector3> CreateSpawnPoints(int playerCount)
    {
        List<Vector3> spawnPoints = new List<Vector3>();

        for (int i = 0; i < playerCount; i++)
        {
            spawnPoints.AddRange(CreateSpawnPointsForPlayer(i+1));
        }

        return spawnPoints;
    }

    private List<Vector3> CreateSpawnPointsForPlayer(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return new List<Vector3>
                {
                   new Vector3(width - 1, 1, 0f),
             new Vector3(width - 2, 0, 0f),
             new Vector3(width - 1, 0, 0f),
                };
            case 2:
                return new List<Vector3>
                {
                new Vector3(1, height - 1, 0f),
             new Vector3(0, height - 2, 0f),
             new Vector3(0, height - 1, 0f),
                };
            case 3:
                return new List<Vector3>
                {
                   new Vector3(width - 1, height - 1, 0f),
             new Vector3(width - 2, height - 1, 0f),
             new Vector3(width-1 , height-2 , 0f),
                };
            case 4:
                return new List<Vector3>
                {
                  new Vector3(0, 1, 0f),
             new Vector3(1, 0, 0f),
             new Vector3(0, 0, 0f),
                };
            default:
                return new List<Vector3>();
        }
    }
}
/*
 //Верхній кут лівий
new Vector3(width - 1, 1, 0f),
             new Vector3(width - 2, 0, 0f),
             new Vector3(width - 1, 0, 0f),
            //Нижній кут правий
             new Vector3(1, height - 1, 0f),
             new Vector3(0, height - 2, 0f),
             new Vector3(0, height - 1, 0f),
             //Верхній кут правий
             new Vector3(width - 1, height - 1, 0f),
             new Vector3(width - 2, height - 1, 0f),
             new Vector3(width-1 , height-2 , 0f),
             //Нижній кут лівий
              new Vector3(0, 1, 0f),
             new Vector3(1, 0, 0f),
             new Vector3(0, 0, 0f),
        };
*/