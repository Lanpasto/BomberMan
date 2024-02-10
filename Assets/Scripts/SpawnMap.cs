using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    [SerializeField] private BlockBehaviour BlockPrefab;
    [SerializeField] private BlockDeScription EmptyBlock;
    [SerializeField] private BlockDeScription Bedrock;
    [SerializeField] private BlockDeScription Brick;

    [SerializeReference] private int width = 22;
    [SerializeReference] private float spawnProbability = 0.5f;
    [SerializeReference] private int height = 12;
    private List<Vector3> validCoordinates;
    private BlockBehaviour[,] mapInfo;
    
    private void Start()
    {
        GenerateMap();
      
    }

    public void GenerateMap(){
        mapInfo = new BlockBehaviour[width, height];
         Vector3[,] coordinates = SpawnBrick();
        List<Vector3> excludedCoordinates = CreateExcludedCoordinatesList();
        List<Vector3> validCoordinates1 = GenerateUnBreakWall(coordinates, excludedCoordinates);
        GenerateBricks(coordinates, excludedCoordinates, validCoordinates1);
    }
        //Спавн гравців
    private List<Vector3> CreateExcludedCoordinatesList()
    {
        return new List<Vector3>
        {
             new Vector3(width - 1, 1, 0f),
             new Vector3(width - 2, 0, 0f),
             new Vector3(width - 1, 0, 0f),
             new Vector3(1, height - 1, 0f),
             new Vector3(0, height - 2, 0f),
             new Vector3(0, height - 1, 0f),
        };

    }
   private Vector3[,] SpawnBrick()
{
    Vector3[,] coordinates = new Vector3[width, height];
    List<Vector3> excludedCoordinates = CreateExcludedCoordinatesList();
    List<Vector3> validCoordinates1 = GenerateUnBreakWall1(coordinates, excludedCoordinates);

    for (int i = 0; i < width; ++i)
    {
        for (int z = 0; z < height; ++z)
        {
            Vector3 currentCoordinate = new Vector3(i, z, 0f);
            if (!validCoordinates1.Contains(currentCoordinate))
            {
                coordinates[i, z] = currentCoordinate;
                InstantiateBlock(coordinates[i, z], EmptyBlock,new Vector2(i,z));
            }
                       // Debug.Log("Coordinate at (" + i + ", " + z + "): " + coordinates[i, z]);

        }
    }

    CreateFrame(coordinates);
    return coordinates;
}

    //Рамка довкола
    private void CreateFrame(Vector3[,] coordinates)
    {
        for (int i = -1; i <= width; ++i)
        {
            for (int z = -1; z <= height; ++z)
            {

                if (i >= 0 && i < width && z >= 0 && z < height)
                {
                    continue;
                }


                InstantiateBlock(new Vector3(i, z, 0f), Bedrock,Vector2.zero,true);
            }
        }
    }


    
    //Незламні кірпічі
    private List<Vector3> GenerateUnBreakWall(Vector3[,] coordinates, List<Vector3> excludedCoordinates)
    {
        List<Vector3> validCoordinates = FillValidCoordinates(coordinates, excludedCoordinates);
        WriteValidCoordinates(validCoordinates, coordinates);
        return validCoordinates;
    }
       private List<Vector3> GenerateUnBreakWall1(Vector3[,] coordinates, List<Vector3> excludedCoordinates)
    {
        List<Vector3> validCoordinates = FillValidCoordinates(coordinates, excludedCoordinates);
        return validCoordinates;
    }

    private List<Vector3> FillValidCoordinates(Vector3[,] coordinates, List<Vector3> excludedCoordinates)
    {
        List<Vector3> validCoordinates = new List<Vector3>();

        for (int i = 0; i < coordinates.GetLength(0); i++)
        {
            for (int d = 0; d < coordinates.GetLength(1); d++)
            {
                if ((i % 2 != 0 || i == 1) && (d % 2 != 0 || d == 1) && !excludedCoordinates.Contains(new Vector3(i, d, 0f)))
                {
                    validCoordinates.Add(new Vector3(i, d, 0f));
                }
            }
        }

        return validCoordinates;
    }
    //Запис координат
    private void WriteValidCoordinates(List<Vector3> validCoordinates, Vector3[,] coordinates)
    {
        if (validCoordinates == null || coordinates == null)
        {
            Debug.LogError("One or both input parameters are null.");
            return;
        }

        foreach (Vector3 coordinate in validCoordinates)
        {
            if (coordinate.x < 0 || coordinate.x >= coordinates.GetLength(0) ||
                coordinate.y < 0 || coordinate.y >= coordinates.GetLength(1))
            {
                Debug.LogError("Invalid coordinate: " + coordinate);
                continue;
            }

            coordinates[(int)coordinate.x, (int)coordinate.y] = coordinate;
            InstantiateBlock(coordinate, Bedrock, new Vector2(coordinate.x,coordinate.y));
        }
    }
    //Cтворення Кірпічів
    private void GenerateBricks(Vector3[,] coordinates, List<Vector3> validCoordinates, List<Vector3> validCoordinates1)
    {
        foreach (Vector3 coordinate in coordinates)
        {
            if (validCoordinates.Contains(coordinate) || validCoordinates1.Contains(coordinate))
            {
                continue;
            }
            RandomInstantiateObject(coordinate, Brick, spawnProbability,coordinates);
        }
    }
    //Рандомайзер    
    private void RandomInstantiateObject(Vector3 coordinate, BlockDeScription objectToInstantiate,
     float spawnProbability,Vector3[,] coordinates )
    {

        float spawnChance = Random.Range(0f, 1f);


        if (spawnChance < spawnProbability)
        {
            InstantiateBlock(coordinate, objectToInstantiate,FindCoordinateIndices(coordinate,coordinates));
        }
    }

    private void InstantiateBlock(Vector3 position, BlockDeScription description,Vector2  cordination, bool isFrame = false)
    {
        GameObject obj = Instantiate(BlockPrefab.gameObject, position, Quaternion.identity);
        obj.GetComponent<BlockBehaviour>().InitializeBlock(description);
        obj.transform.SetParent(this.transform);
        

        if (!isFrame)
        {
            mapInfo[Mathf.CeilToInt(cordination.x), Mathf.CeilToInt(cordination.y)] = obj.GetComponent<BlockBehaviour>();
        }
    }
   private Vector2 FindCoordinateIndices(Vector3 coordinate,Vector3[,] coordinates )
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Check if the coordinate matches the position of the block in the array
                if (coordinates[i, j].x == coordinate.x &&
                    coordinates[i, j].y == coordinate.y)
                {
                    return new Vector2(i, j); // Return the indices if found
                }
            }
        }

        return Vector2.zero;
    }
    private void PrintMassive()
    {
        // Loop through the rows of the 2D array
        for (int i = 0; i < width; i++)
        {
            string rowString = "";

            // Loop through the columns of the 2D array
            for (int j = 0; j < height; j++)
            {
                // Add the string representation of the element to the row string
                rowString += "[" + i + "," + j + "]: " + mapInfo[i, j].ToString() + "  ";
            }

            // Print the row string
            Debug.Log(rowString);
        }
    }
}