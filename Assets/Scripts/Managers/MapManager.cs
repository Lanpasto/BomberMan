using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private BlockBehaviour[,] mapInfo;
    private BlockBehaviour[,] emptyBlockInfo;

    public void SetMap(SpawnMap.MapPartsInfo generatedMap)
    {
        mapInfo = new BlockBehaviour[generatedMap.MapInfo.GetLength(0), generatedMap.MapInfo.GetLength(1)];
        mapInfo = generatedMap.MapInfo;
        
        emptyBlockInfo = new BlockBehaviour[generatedMap.EmptyBlockInfo.GetLength(0), generatedMap.EmptyBlockInfo.GetLength(1)];
        emptyBlockInfo = generatedMap.EmptyBlockInfo;
    }

    public BlockBehaviour[,] GetMap()
    {
        return mapInfo;
    }

    public void RegisterNewBlock(BlockBehaviour blockBehaviour)
    {
        mapInfo[(int)blockBehaviour.coordinates.x, (int)blockBehaviour.coordinates.y] = blockBehaviour;
    }

    public void UnRegisterBlock(Vector2 coordinates)
    {
        mapInfo[(int)coordinates.x, (int)coordinates.y] = emptyBlockInfo[(int)coordinates.x, (int)coordinates.y];
    }

    public BlockBehaviour GetBlockBehaviour(Vector2 coordinates)
    {
        return mapInfo[(int)coordinates.x, (int)coordinates.y];
    }


}
