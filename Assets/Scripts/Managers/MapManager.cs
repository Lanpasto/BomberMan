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

    public void SetMap(BlockBehaviour[,] generatedMap)
    {

        mapInfo = new BlockBehaviour[generatedMap.GetLength(0), generatedMap.GetLength(1)];
        mapInfo = generatedMap;
    }

    public BlockBehaviour[,] GetMap()
    {
        return mapInfo;
    }

    public void ChangeMap()
    {
        
    }


}
