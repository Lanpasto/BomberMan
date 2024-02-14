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


    [SerializeReference] private float spawnProbability = 0.5f;
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
        newMap = new MapUnit[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                newMap[x, y] = new MapUnit();
            }
        }


        List<Vector3> spawnPlayerCoordinatesException = CreatespawnPlayerCoordinatesExceptionList(countOfPlayer);
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

    //Спавн гравців
    private List<Vector3> CreatespawnPlayerCoordinatesExceptionList(int countOfPlayer)
    {
        List<Vector3> spawnCoordinates = new List<Vector3>();

        switch (countOfPlayer)
        {
            case 2:

                spawnCoordinates.AddRange(SpawnCoordinatesSetForTwoPlayers());
                break;
            case 3:
                int randomIndex = BrickGeneratePattern.GetRandomIndex();
                if (randomIndex == 0)
                {

                    spawnCoordinates.AddRange(SpawnCoordinatesSetForThreePlayersFirstCase());
                }
                else
                {

                    spawnCoordinates.AddRange(SpawnCoordinatesSetForThreePlayersSecondCase());
                }
                break;
            case 4:
                spawnCoordinates.AddRange(SpawnCoordinatesSetForFourPlayers());

                break;
        }

        return spawnCoordinates;
    }

    private List<Vector3> SpawnCoordinatesSetForTwoPlayers()
    {
        return new List<Vector3>
        {   
            //Верхній кут лівий
             new Vector3(width - 1, 1, 0f),
             new Vector3(width - 2, 0, 0f),
             new Vector3(width - 1, 0, 0f),
            //Нижній кут правий
             new Vector3(1, height - 1, 0f),
             new Vector3(0, height - 2, 0f),
             new Vector3(0, height - 1, 0f),
        };
    }
    private List<Vector3> SpawnCoordinatesSetForThreePlayersFirstCase()
    {
        return new List<Vector3>
        {    
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
        };
    }
    private List<Vector3> SpawnCoordinatesSetForThreePlayersSecondCase()
    {
        return new List<Vector3>
        {   
            //Верхній кут лівий
             new Vector3(width - 1, 1, 0f),
             new Vector3(width - 2, 0, 0f),
             new Vector3(width - 1, 0, 0f),
            //Нижній кут правий
             new Vector3(1, height - 1, 0f),
             new Vector3(0, height - 2, 0f),
             new Vector3(0, height - 1, 0f),
              //Нижній кут лівий
              new Vector3(0, 1, 0f),
             new Vector3(1, 0, 0f),
             new Vector3(0, 0, 0f),
        };
    }
    private List<Vector3> SpawnCoordinatesSetForFourPlayers()
    {
        return new List<Vector3>
        {
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
        foreach (Vector3 currentCoordinate in coordinatesOfMap)
        {
            float widthOfobstacle = 1f; // Налаштуйте тут ширину фігур
            if (!(spawnPlayerCoordinatesException.Contains(currentCoordinate) || unbreakableWallCoordinatesException.Contains(currentCoordinate)))
            {
                switch (randomType)
                {
                    case RandomTypeEnum.PatternDiagonal:
                        if (BrickGeneratePattern.IsOnDiagonalPatternFromLeftTopToRightBottom(currentCoordinate, widthOfobstacle))
                        {
                            TrySpawnBlock(currentCoordinate, coordinatesOfMap);
                        }
                        break;
                    case RandomTypeEnum.PatternX:
                        if (BrickGeneratePattern.IsOnDiagonalsPatternX(currentCoordinate, width, height, widthOfobstacle))
                        {
                            TrySpawnBlock(currentCoordinate, coordinatesOfMap);
                        }
                        break;
                    case RandomTypeEnum.IsOnLine:
                        int randomIndex = BrickGeneratePattern.GetRandomIndex();
                        if (randomIndex == 0)
                        {

                            if (BrickGeneratePattern.IsOnLine(currentCoordinate, width, height, widthOfobstacle, true))
                            {
                                TrySpawnBlock(currentCoordinate, coordinatesOfMap);
                            }
                        }
                        else
                        {

                            if (BrickGeneratePattern.IsOnLine(currentCoordinate, width, height, widthOfobstacle, false))
                            {
                                TrySpawnBlock(currentCoordinate, coordinatesOfMap);

                            }
                        }
                        break;
                    case RandomTypeEnum.Default:
                        TrySpawnBlock(currentCoordinate, coordinatesOfMap);

                        break;
                }
            }
        }
    }

    private void TrySpawnBlock(Vector3 currentCoordinate, Vector3[,] coordinatesOfMap)
    {
        if (BrickGeneratePattern.GetSpawnChance() < spawnProbability)
        {
            Vector2 coordinateIndices = FindCoordinateIndices(currentCoordinate, coordinatesOfMap);
            InstantiateBlock(currentCoordinate, Brick, coordinateIndices);
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
    private void PrintMassive()
    {
        for (int coordinateX = 0; coordinateX < width; coordinateX++)
        {
            string rowString = "";

            for (int coordinateY = 0; coordinateY < height; coordinateY++)
            {
                rowString += "[" + coordinateX + "," + coordinateY + "]: " + mapInfo[coordinateX, coordinateY].ToString() + "  ";
            }

            Debug.Log(rowString);
        }
    }
}
