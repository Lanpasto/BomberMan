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

    private EntityBehaviour[,] mapInfo;
    private EntityBehaviour[,] emptyBlockInfo;

    public void SetMap(SpawnMap.MapPartsInfo generatedMap)
    {
        mapInfo = new EntityBehaviour[generatedMap.MapInfo.GetLength(0), generatedMap.MapInfo.GetLength(1)];
        mapInfo = generatedMap.MapInfo;

        emptyBlockInfo = new EntityBehaviour[generatedMap.EmptyBlockInfo.GetLength(0), generatedMap.EmptyBlockInfo.GetLength(1)];
        emptyBlockInfo = generatedMap.EmptyBlockInfo;
    }

    public EntityBehaviour[,] GetMap()
    {
        return mapInfo;
    }

    public void RegisterNewBlock(EntityBehaviour entityBehaviour)
    {
        mapInfo[(int)entityBehaviour.coordinates.x, (int)entityBehaviour.coordinates.y] = entityBehaviour;
    }

    public void UnRegisterBlock(Vector2 coordinates)
    {
        if(mapInfo != null && emptyBlockInfo != null)
            mapInfo[(int)coordinates.x, (int)coordinates.y] = emptyBlockInfo[(int)coordinates.x, (int)coordinates.y];
    }

    public EntityBehaviour GetBlockBehaviour(Vector2 coordinates)
    {
        return mapInfo[(int)coordinates.x, (int)coordinates.y];
    }

    public Vector2 GetPositionByCoordinates(Vector2 coordinates)
    {
        return mapInfo[(int)coordinates.x, (int)coordinates.y].transform.position;
    }


}