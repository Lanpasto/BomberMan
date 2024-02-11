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

    private List<Vector3> validCoordinates;
    private EntityBehaviour[,] mapInfo;
    private EntityBehaviour[,] EmptyBlockInfo;

    private int width;
    private int height;
    private int countOfPlayer;

    public class MapPartsInfo
    {
        public EntityBehaviour[,] MapInfo;
        public EntityBehaviour[,] EmptyBlockInfo;

        public MapPartsInfo(EntityBehaviour[,] mapInfo, EntityBehaviour[,] emptyBlockInfo, int height, int width)
        {
            this.EmptyBlockInfo = new EntityBehaviour[width, height];
            this.MapInfo = new EntityBehaviour[width, height];

            EmptyBlockInfo = emptyBlockInfo;
            MapInfo = mapInfo;
        }
    }

    public MapPartsInfo GenerateMap(int width, int height, int countOfPlayer)
    {
        this.width = width;
        this.height = height;
        this.countOfPlayer = countOfPlayer;

        EmptyBlockInfo = new EntityBehaviour[width, height];
        mapInfo = new EntityBehaviour[width, height];
        List<Vector3> spawnCoordinatesException = CreateSpawnCoordinatesExceptionList(countOfPlayer);
        Vector3[,] coordinatesOfMap = GenerateEmptyBlock(spawnCoordinatesException);
        List<Vector3> unbreakableWallCoordinatesException = GenerateUnBreakWall(coordinatesOfMap, spawnCoordinatesException);
        GenerateBricks(coordinatesOfMap, spawnCoordinatesException, unbreakableWallCoordinatesException);

        MapPartsInfo newMap = new MapPartsInfo(mapInfo, EmptyBlockInfo, height, width);

        return newMap;
    }
    private Vector3[,] GenerateEmptyBlock(List<Vector3> spawnCoordinatesException)
    {
        Vector3[,] coordinatesOfMap = new Vector3[width, height];
        List<Vector3> unbreakableWallCoordinatesException = FillValidCoordinatesForUnBreakWall(coordinatesOfMap, spawnCoordinatesException);

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
                // Debug.Log("Coordinate at (" + i + ", " + z + "): " + coordinates[i, z]);

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
    private List<Vector3> CreateSpawnCoordinatesExceptionList(int countOfPlayer)
    {
        List<Vector3> spawnCoordinates = new List<Vector3>();

        switch (countOfPlayer)
        {
            case 2:

                spawnCoordinates.AddRange(SpawnCoordinatesSetForTwoPlayers());
                break;
            case 3:

                System.Random random = new System.Random();
                int randomIndex = random.Next(2);
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
    private List<Vector3> GenerateUnBreakWall(Vector3[,] coordinatesOfMap, List<Vector3> spawnCoordinatesException)
    {
        List<Vector3> validCoordinatesOfUnBreakWall = FillValidCoordinatesForUnBreakWall(coordinatesOfMap, spawnCoordinatesException);
        SpawnUnBreakWallOnValidCoordinates(coordinatesOfMap, validCoordinatesOfUnBreakWall);
        return validCoordinatesOfUnBreakWall;
    }

    private List<Vector3> FillValidCoordinatesForUnBreakWall(Vector3[,] coordinatesOfMap, List<Vector3> spawnCoordinatesException)
    {
        List<Vector3> validCoordinatesOfUnBreakWall = new List<Vector3>();

        for (int coordinateX = 0; coordinateX < coordinatesOfMap.GetLength(0); coordinateX++)
        {
            for (int coordinateY = 0; coordinateY < coordinatesOfMap.GetLength(1); coordinateY++)
            {
                if ((coordinateX % 2 != 0 || coordinateX == 1) && (coordinateY % 2 != 0 || coordinateY == 1) && !spawnCoordinatesException.Contains(new Vector3(coordinateX, coordinateY, 0f)))
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
    private void GenerateBricks(Vector3[,] coordinatesOfMap, List<Vector3> spawnCoordinatesException, List<Vector3> unbreakableWallCoordinatesException)
    {
        foreach (Vector3 coordinate in coordinatesOfMap)
        {
            if (spawnCoordinatesException.Contains(coordinate) || unbreakableWallCoordinatesException.Contains(coordinate))
            {
                continue;
            }
            RandomInstantiateObject(coordinate, Brick, spawnProbability, coordinatesOfMap);
        }
    }
    //Рандомайзер    
    private void RandomInstantiateObject(Vector3 coordinatesOfMap, EntityDescription objectToInstantiate,
     float spawnProbability, Vector3[,] coordinates)
    {

        float spawnChance = Random.Range(0f, 1f);


        if (spawnChance < spawnProbability)
        {
            InstantiateBlock(coordinatesOfMap, objectToInstantiate, FindCoordinateIndices(coordinatesOfMap, coordinates));
        }
    }

    private void InstantiateBlock(Vector3 position, EntityDescription description, Vector2 cordination, bool isFrame = false)
    {
        GameObject obj = Instantiate(placeableEntityPrefab.gameObject, position, Quaternion.identity);
        obj.GetComponent<PlaceableEntityBehaviour>().Initialize(description, cordination);
        obj.transform.SetParent(this.transform);


        if (!isFrame)
        {
            mapInfo[(int)cordination.x, (int)cordination.y] = obj.GetComponent<EntityBehaviour>();
            if (description == EmptyBlock)
            {
                EmptyBlockInfo[(int)cordination.x, (int)cordination.y] = obj.GetComponent<EntityBehaviour>();
            }
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