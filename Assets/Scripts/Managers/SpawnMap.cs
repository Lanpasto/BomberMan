using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnMap : MonoBehaviour
{
    [FormerlySerializedAs("BlockPrefab")]
    [SerializeField] private PlaceableEntityBehaviour placeableEntityPrefab;
    [SerializeField] private EntityDescription EmptyBlock;
    [SerializeField] private EntityDescription Bedrock;
    [SerializeField] private EntityDescription Brick;
    [SerializeReference] private float spawnProbability;
    private EntityBehaviour[,] mapInfo;
    private MapUnit[,] newMap;
    private int width;
    private int height;
    private int countOfPlayer;
    RandomTypeEnum randomType;

    public MapUnit[,] GenerateMap(int width, int height, int countOfPlayer, RandomTypeEnum randomType)
    {
        this.width = width;
        this.height = height;
        this.countOfPlayer = countOfPlayer;
        PlayerSpawnPointFactory spawnPointFactory = new PlayerSpawnPointFactory(width, height);
        List<Vector3> spawnPlayerCoordinatesException = spawnPointFactory.CreateSpawnPoints(countOfPlayer);

        newMap = new MapUnit[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                newMap[x, y] = new MapUnit();
            }
        }

        Vector3[,] coordinatesOfMap = GenerateEmptyBlock(spawnPlayerCoordinatesException);
        List<Vector3> unbreakableWallCoordinatesException = GenerateUnBreakWall(coordinatesOfMap, spawnPlayerCoordinatesException);
        GenerateBricks(coordinatesOfMap, spawnPlayerCoordinatesException, unbreakableWallCoordinatesException, randomType);


        return newMap;
    }
    private Vector3[,] GenerateEmptyBlock(List<Vector3> spawnPlayerCoordinatesException)
    {
        Vector3[,] coordinatesOfMap = new Vector3[width, height];
        List<Vector3> unbreakableWallCoordinatesException = FillListOfValidCoordinatesForUnBreakWall(coordinatesOfMap, spawnPlayerCoordinatesException);

        for (int coordinateX = 0; coordinateX < width; ++coordinateX)
        {
            for (int coordinateY = 0; coordinateY < height; ++coordinateY)
            {
                Vector3 currentCoordinate = new Vector3(coordinateX, coordinateY, 0f);
                if (!unbreakableWallCoordinatesException.Contains(currentCoordinate))
                {
                    coordinatesOfMap[coordinateX, coordinateY] = currentCoordinate;
                    InstantiateBlock(coordinatesOfMap[coordinateX, coordinateY], EmptyBlock, new Vector2(coordinateX, coordinateY));

                }
            }
        }

        GenerateFrameOfUnBreakWall();
        return coordinatesOfMap;
    }

    //Рамка довкола
    private void GenerateFrameOfUnBreakWall()
    {
        for (int coordinateX = -1; coordinateX <= width; ++coordinateX)
        {
            for (int coordinateY = -1; coordinateY <= height; ++coordinateY)
            {

                if (coordinateX >= 0 && coordinateX < width && coordinateY >= 0 && coordinateY < height)
                {
                    continue;
                }
                InstantiateBlock(new Vector3(coordinateX, coordinateY, 0f), Bedrock, new Vector2(-1, -1), true);
            }
        }
    }
    //Незламні кірпічі
    private List<Vector3> GenerateUnBreakWall(Vector3[,] coordinatesOfMap, List<Vector3> spawnPlayerCoordinatesException)
    {
        List<Vector3> validCoordinatesOfUnBreakWall = FillListOfValidCoordinatesForUnBreakWall(coordinatesOfMap, spawnPlayerCoordinatesException);
        SpawnUnBreakWallOnValidCoordinates(coordinatesOfMap, validCoordinatesOfUnBreakWall);
        return validCoordinatesOfUnBreakWall;
    }

    private List<Vector3> FillListOfValidCoordinatesForUnBreakWall(Vector3[,] coordinatesOfMap, List<Vector3> spawnPlayerCoordinatesException)
    {
        List<Vector3> validCoordinatesOfUnBreakWall = new List<Vector3>();

        for (int coordinateX = 0; coordinateX < coordinatesOfMap.GetLength(0); coordinateX++)
        {
            for (int coordinateY = 0; coordinateY < coordinatesOfMap.GetLength(1); coordinateY++)
            {
                if ((coordinateX % 2 != 0 || coordinateX == 1) && (coordinateY % 2 != 0 || coordinateY == 1) && !spawnPlayerCoordinatesException.Contains(new Vector3(coordinateX, coordinateY, 0f)))
                {
                    validCoordinatesOfUnBreakWall.Add(new Vector3(coordinateX, coordinateY, 0f));
                }
            }
        }

        return validCoordinatesOfUnBreakWall;
    }

    //Запис координат
    private void SpawnUnBreakWallOnValidCoordinates(Vector3[,] coordinatesOfMap, List<Vector3> validCoordinatesOfUnBreakWall)
    {
        if (validCoordinatesOfUnBreakWall == null || coordinatesOfMap == null)
        {
            Debug.LogError("One or both input parameters are null.");
            return;
        }

        foreach (Vector3 coordinate in validCoordinatesOfUnBreakWall)
        {
            if (coordinate.x < 0 || coordinate.x >= coordinatesOfMap.GetLength(0) ||
                coordinate.y < 0 || coordinate.y >= coordinatesOfMap.GetLength(1))
            {
                Debug.LogError("Invalid coordinate: " + coordinate);
                continue;
            }

            coordinatesOfMap[(int)coordinate.x, (int)coordinate.y] = coordinate;
            InstantiateBlock(coordinate, Bedrock, new Vector2(coordinate.x, coordinate.y));
        }
    }
    //Cтворення Кірпічів
    private void GenerateBricks(Vector3[,] coordinatesOfMap, List<Vector3> spawnPlayerCoordinatesException, List<Vector3> unbreakableWallCoordinatesException, RandomTypeEnum randomType)
    {
        List<Vector3> eligibleSpawnPoints = new List<Vector3>();

        foreach (Vector3 currentCoordinate in coordinatesOfMap)
        {
            float widthOfObstacle = Mathf.Min(width, height) * 0.03f;
            if (!(spawnPlayerCoordinatesException.Contains(currentCoordinate) || unbreakableWallCoordinatesException.Contains(currentCoordinate)))
            {
                switch (randomType)
                {
                    case RandomTypeEnum.PatternX:
                        SpawnBrickIfNeeded(currentCoordinate, width, height, widthOfObstacle, (coord, w, h, obstacleWidth) => BrickGeneratePattern.IsOnDiagonalsPatternX(coord, width, height, obstacleWidth), eligibleSpawnPoints, coordinatesOfMap);
                        break;
                    case RandomTypeEnum.PatternDiagonal:
                        SpawnBrickIfNeeded(currentCoordinate, width, height, widthOfObstacle, (coord, w, h, obstacleWidth) => BrickGeneratePattern.IsOnDiagonalPatternRightBottom(coord, width, height, obstacleWidth), eligibleSpawnPoints, coordinatesOfMap);
                        break;
                    case RandomTypeEnum.IsOnLine:
                        SpawnBrickIfNeeded(currentCoordinate, width, height, widthOfObstacle, BrickGeneratePattern.IsOnLine, eligibleSpawnPoints, coordinatesOfMap);
                        break;
                    case RandomTypeEnum.Default:
                        TrySpawnBlock(currentCoordinate, coordinatesOfMap);
                        break;
                }
            }
        }
    }
    private void SpawnBrickIfNeeded(Vector3 currentCoordinate, float width, float height, float widthOfObstacle, Func<Vector3, float, float, float, bool> checkPattern, List<Vector3> eligibleSpawnPoints, Vector3[,] coordinatesOfMap)
    {
        if (checkPattern(currentCoordinate, width, height, widthOfObstacle))
        {
            if (BrickGeneratePattern.GetSpawnChance() < spawnProbability)
            {
                Vector2 coordinateIndices = FindCoordinateIndices(currentCoordinate, coordinatesOfMap);
                InstantiateBlock(currentCoordinate, Brick, coordinateIndices);
                eligibleSpawnPoints.Add(currentCoordinate);
            }
        }
        SpawnBlockBasedOnDistance(currentCoordinate, eligibleSpawnPoints, coordinatesOfMap);
    }
    private void SpawnBlockBasedOnDistance(Vector3 currentCoordinate, List<Vector3> eligibleSpawnPoints, Vector3[,] coordinatesOfMap)
    {
        float minDistance = float.MaxValue;

        foreach (Vector3 spawnPoint in eligibleSpawnPoints)
        {
            float distance = Vector3.Distance(currentCoordinate, spawnPoint);

            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }
        float spawnChance = BrickGeneratePattern.CalculateSpawnChance(minDistance);
        if (Random.Range(0.0f, 1.0f) < spawnChance)
        {
            SetBrick(currentCoordinate, coordinatesOfMap);
        }
    }
    private void SetBrick(Vector3 currentCoordinate, Vector3[,] coordinatesOfMap)
    {
        Vector2 coordinateIndices = FindCoordinateIndices(currentCoordinate, coordinatesOfMap);
        InstantiateBlock(currentCoordinate, Brick, coordinateIndices);
    }

    private void TrySpawnBlock(Vector3 currentCoordinate, Vector3[,] coordinatesOfMap)
    {
        if (BrickGeneratePattern.GetSpawnChance() < spawnProbability)
        {
            SetBrick(currentCoordinate, coordinatesOfMap);
        }
    }



    private void InstantiateBlock(Vector3 position, EntityDescription description, Vector2 cordination, bool isFrame = false)
    {
        GameObject obj = Instantiate(placeableEntityPrefab.gameObject, position, Quaternion.identity);
        obj.GetComponent<PlaceableEntityBehaviour>().Initialize(description, cordination);
        obj.transform.SetParent(this.transform);


        if (!isFrame)
        {
            newMap[(int)cordination.x, (int)cordination.y].Add(obj.GetComponent<EntityBehaviour>());
        }
    }

    private Vector2 FindCoordinateIndices(Vector3 coordinate, Vector3[,] coordinates)
    {
        for (int coordinateX = 0; coordinateX < width; coordinateX++)
        {
            for (int coordinateY = 0; coordinateY < height; coordinateY++)
            {
                if (coordinates[coordinateX, coordinateY].x == coordinate.x &&
                    coordinates[coordinateX, coordinateY].y == coordinate.y)
                {
                    return new Vector2(coordinateX, coordinateY);
                }
            }
        }

        return Vector2.zero;
    }
}
