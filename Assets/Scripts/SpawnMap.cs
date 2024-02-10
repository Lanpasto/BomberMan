using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    [SerializeReference] private GameObject prefab;
    [SerializeReference] private GameObject prefab1;
    [SerializeReference] private GameObject prefab2;
    [SerializeReference] private GameObject prefab3;
    [SerializeReference] private int width = 22;
    [SerializeReference] private float spawnProbability = 0.5f;

    [SerializeReference] private int height = 12;
    private List<Vector3> validCoordinates;

    private void Start()
    {
        Vector3[,] coordinates = SpawnBrick();
        List<Vector3> excludedCoordinates = CreateExcludedCoordinatesList();
        List<Vector3> validCoordinates1 = GenerateUnBreakWall(coordinates, excludedCoordinates);
        GenerateBricks(coordinates, excludedCoordinates, validCoordinates1);
    }
    private Vector3[,] SpawnBrick()
    {

        Vector3[,] coordinates = new Vector3[width, height];

        for (int i = 0; i < width; ++i)
        {
            for (int z = 0; z < height; ++z)
            {

                coordinates[i, z] = new Vector3(i, z, 0f);
                GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
                obj.transform.position = new Vector3(i, z, 0f);
                InstantiateBrickPrefab(coordinates[i, z]);
                //  Debug.Log("Coordinates[" + i + "," + z + "]: " + coordinates[i, z]);
            }

        }
        // Додаємо рамку навколо прямокутної області
        CreateFrame(coordinates);
        return coordinates;
    }
    private void InstantiateBrickPrefab(Vector3 position)
    {
        // Створюємо прямокутні об'єкти в прямокутній області
        Instantiate(prefab, position, Quaternion.identity);
    }

    private void CreateFrame(Vector3[,] coordinates)
    {
        for (int i = -1; i <= width; ++i)
        {
            for (int z = -1; z <= height; ++z)
            {
                // Якщо координата знаходиться всередині прямокутної області, пропустити
                if (i >= 0 && i < width && z >= 0 && z < height)
                {
                    continue;
                }

                // Інакше, створити прямокутний об'єкт для утворення рамки
                Instantiate(prefab2, new Vector3(i, z, 0f), Quaternion.identity);
            }
        }
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
    //Незламні кірпічі
    private List<Vector3> GenerateUnBreakWall(Vector3[,] coordinates, List<Vector3> excludedCoordinates)
    {
        List<Vector3> validCoordinates = FillValidCoordinates(coordinates, excludedCoordinates);
        WriteValidCoordinates(validCoordinates, coordinates);

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
            Instantiate(prefab2, coordinate, Quaternion.identity);
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
        //Instantiate(prefab3, coordinate, Quaternion.identity);
        RandomInstantiateObject(coordinate, prefab3, spawnProbability);
        }
    }
    private void RandomInstantiateObject(Vector3 coordinate, GameObject objectToInstantiate, float spawnProbability)
{
    // Генеруємо випадкове число в діапазоні від 0 до 1
    float spawnChance = Random.Range(0f, 1f);

    // Перевіряємо, чи випадкове число менше заданої ймовірності
    if (spawnChance < spawnProbability)
    {
        // Якщо так, то створюємо об'єкт за допомогою Instantiate
        Instantiate(objectToInstantiate, coordinate, Quaternion.identity);
    }
}

    private void InstantiateBlock1(Vector3 position, GameObject blockPrefab3)
    {
        GameObject obj = Instantiate(blockPrefab3, position, Quaternion.identity);
    }

    private void InstantiateBlock(Vector3 position, GameObject blockPrefab2)
    {
        GameObject obj = Instantiate(blockPrefab2, position, Quaternion.identity);
    }
}
    // private void PlayerSpawnPlace(Vector3[,] coordinates)
    // {

    //     coordinates[0, 12] = new Vector3(width - 1, 1, 0f);
    //     coordinates[1, 11] = new Vector3(width - 2, 0, 0f);
    //     coordinates[2, 12] = new Vector3(width - 1, 0, 0f);


    //     coordinates[12, 0] = new Vector3(1, height - 1, 0f);
    //     coordinates[11, 1] = new Vector3(0, height - 2, 0f);
    //     coordinates[12, 2] = new Vector3(0, height - 1, 0f);


    //     Instantiate(prefab1, coordinates[0, 12], Quaternion.identity);
    //     Instantiate(prefab1, coordinates[1, 11], Quaternion.identity);
    //     Instantiate(prefab1, coordinates[2, 12], Quaternion.identity);

    //     Instantiate(prefab1, coordinates[12, 0], Quaternion.identity);
    //     Instantiate(prefab1, coordinates[11, 1], Quaternion.identity);
    //     Instantiate(prefab1, coordinates[12, 2], Quaternion.identity);
    // }
// private void SpawnBlocksEveryTwo(Vector3[,] coordinates)
// {
// for (int i = 1; i < coordinates.GetLength(0); i += 2) 
// {   

//     InstantiateBlock(coordinates[i, i+2]);
//     InstantiateBlock(coordinates[i, i+2]);
//     InstantiateBlock(coordinates[i, i+2]);
//     InstantiateBlock(coordinates[i, i+2]);
//     InstantiateBlock(coordinates[i, i+2]);
// }
// }
// private void SpawnSpawnFirstPlayer(){
//       GameObject[,] matrix = new GameObject[1, 11];


//     for (int i = 0; i < width; ++i)
//     {

//         for (int z = 0; z < height; ++z)
//         {
//             GameObject obj = Instantiate(prefab1, transform.position, Quaternion.identity);
//             obj.transform.position = new Vector3(i, z, 0f);
//             matrix[i, z] = obj;
//         }
//     }
// }
// private void SpawnSpawnUnBreakWall(){
//      for (int y = 1; y > 1; y--)
//     {

//     }
// }
// private void SpawnSpawnFirstPlayer()
// {      
//     int squareSize = 2;
//     int startX = 0;
//     int startY = height-1;
//     for (int y = startY; y > startY - squareSize; --y)
//     {

//         for (int x = startX; x < startX + squareSize; x++)
//         {

//             GameObject obj = Instantiate(prefab1, transform.position, Quaternion.identity);
//             obj.transform.position = new Vector3(x, y, 0f);
//         }
//     }
// }
// private void SpawnSpawnSecondPlayer()
// {
//     int squareSize = 2;
//     int startX = width - squareSize; // startX буде ширина мінус розмір квадрата
//     int startY = 1;

//     for (int y = startY; y > startY - squareSize; y--)
//     {
//         for (int x = startX; x < startX + squareSize; x++)
//         {
//             GameObject obj = Instantiate(prefab1, transform.position, Quaternion.identity);
//             obj.transform.position = new Vector3(x, y, 0f);
//         }
//     }
// }

//obj.transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
// Створюємо матрицю об'єктів для першої половини
// for (int i = 1; i <= 2; ++i)
// {
//     for (int j = 11; j <= 12; ++j)
//     {
//         GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
//         obj.transform.position = new Vector3(i, j, 0f);
//     }
// }

// // Створюємо матрицю об'єктів для другої половини
// for (int i = 11; i <= 12; ++i)
// {
//     for (int j = 1; j <= 2; ++j)
//     {
//         GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
//         obj.transform.position = new Vector3(i, j, 0f);
//     }
// }